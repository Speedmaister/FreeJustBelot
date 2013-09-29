using FreeJustBelot.Data;
using FreeJustBelot.DataLayer;
using FreeJustBelot.Models;
using FreeJustBelot.WebApi.Hubs;
using FreeJustBelot.WebApi.Models;
using FreeJustBelot.WebApi.Persisters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FreeJustBelot.WebApi.Controllers
{
    public class GamesController : BaseController
    {
        private IRepository<Game> gamesRepository;
        private IRepository<GamesRoom> roomsRepository;
        private IRepository<User> usersRepository;

        public GamesController()
        {
            var dbContext = new FreeJustBelotEntities();
            this.gamesRepository = new Repository<Game>(dbContext);
            this.roomsRepository = new Repository<GamesRoom>(dbContext);
            this.usersRepository = new Repository<User>(dbContext);
        }

        [HttpGet]
        [ActionName("all")]
        public IEnumerable<GameModel> GetAll(string sessionKey)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
            {
                if (!UsersPersister.ValidateSessionKey(sessionKey))
                {
                    throw new ArgumentException("Invalid format of session key");
                }

                var games = this.gamesRepository.All().Where(x => x.PlayersWaiting < 4);
                List<GameModel> gamesCurrentlyHaveFreeSlot =
                    (from game in games
                     select new GameModel
                     {
                         Host = game.User.Nickname,
                         Name = game.Name,
                         PlayersWaiting = game.PlayersWaiting
                     }).ToList();

                return gamesCurrentlyHaveFreeSlot;
            });

            return response;
        }

        [HttpGet]
        [ActionName("room")]
        public RoomModel GetRoom(string sessionKey, string gameName)
        {
            if (!UsersPersister.ValidateSessionKey(sessionKey))
            {
                throw new ArgumentException("Invalid format of session key");
            }

            var game = this.gamesRepository.All()
                .FirstOrDefault(x => x.Name == gameName);

            var room = this.roomsRepository.All()
                .FirstOrDefault(x => x.GameId == game.Id);

            var roomModel = AddPlayersToRoomModeList(room);

            return roomModel;
        }

        private RoomModel AddPlayersToRoomModeList(GamesRoom room)
        {
            string user1 = this.usersRepository.Get((int)room.Player1).Nickname;
            string user2 = null;
            string user3 = null;
            string user4 = null;
            if (room.Player2 != null)
            {
                user2 = this.usersRepository.Get((int)room.Player2).Nickname;
            }

            if (room.Player3 != null)
            {
                user3 = this.usersRepository.Get((int)room.Player3).Nickname;
            }

            if (room.Player4 != null)
            {
                user4 = this.usersRepository.Get((int)room.Player4).Nickname;
            }

            RoomModel roomModel = new RoomModel();
            roomModel.Players.Add(user1);
            roomModel.Players.Add(user2);
            roomModel.Players.Add(user3);
            roomModel.Players.Add(user4);

            return roomModel;
        }

        [HttpPost]
        [ActionName("create")]
        public HttpResponseMessage Create(string sessionKey, CreateGameModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
                {
                    ValidateInputForCreateGame(sessionKey, model);

                    User user = this.gamesRepository.GetUserBySessionKey(sessionKey);

                    var allGames = this.gamesRepository.All();
                    var gameCreatedByCurrentUser = allGames.FirstOrDefault(x => x.HostId == user.Id);
                    if (gameCreatedByCurrentUser != null)
                    {
                        throw new ArgumentException("User already created a game.");
                    }

                    var gameThatAlreadyExists = allGames.FirstOrDefault(x => x.Name == model.Name);
                    if (gameThatAlreadyExists != null)
                    {
                        throw new ArgumentException("Game with this name already exists.");
                    }


                    Game newGame = new Game();
                    newGame.User = user;
                    newGame.PlayersWaiting = 1;
                    newGame.Name = model.Name;
                    if (string.IsNullOrWhiteSpace(model.Password))
                    {
                        newGame.Password = model.Password;
                    }

                    var game = this.gamesRepository.Add(newGame);

                    GamesRoom newRoom = new GamesRoom();
                    newRoom.GameId = game.Id;
                    newRoom.Player1 = user.Id;

                    var room = this.roomsRepository.Add(newRoom);

                    JustBelotHub.games.Add(game);
                    JustBelotHub.rooms.Add(room);

                    return Request.CreateResponse(HttpStatusCode.Created);
                });

            return response;
        }

        [HttpPost]
        [ActionName("join")]
        public HttpResponseMessage Join(string sessionKey, JoinGameModel model)
        {
            var response = this.PerformOperationAndHandleExceptions(() =>
                {
                    ValidateInputForJoinGame(sessionKey, model);

                    User user = this.gamesRepository.GetUserBySessionKey(sessionKey);

                    var game = this.gamesRepository.All()
                        .FirstOrDefault(x => x.Name == model.GameName && x.User.Nickname == model.Host);

                    if (game == null)
                    {
                        throw new ArgumentNullException("Game does not exist.");
                    }

                    var room = this.roomsRepository.All()
                        .FirstOrDefault(x => x.GameId == game.Id);
                    this.SetPlayerAtCurrentPosition(room, user);

                    if (game.Password != null && game.Password != model.Password)
                    {
                        throw new ArgumentException("Incorrect password.");
                    }

                    game.PlayersWaiting++;

                    this.gamesRepository.Update(game.Id, game);
                    this.roomsRepository.Update(room.Id, room);

                    JustBelotHub.games.FirstOrDefault(x => x.Id == game.Id).PlayersWaiting++;

                    return Request.CreateResponse(HttpStatusCode.OK);
                });

            return response;
        }

        private static void ValidateInputForCreateGame(string sessionKey, CreateGameModel model)
        {
            if (!UsersPersister.ValidateSessionKey(sessionKey))
            {
                throw new ArgumentException("Invalid format of session key");
            }

            if (string.IsNullOrWhiteSpace(model.Name))
            {
                throw new ArgumentException("Game needs to have a non-empty name.");
            }

            if (model.Password != null && model.Password.Length == 0)
            {
                throw new ArgumentException("Password cant be white space.");
            }
        }

        private static void ValidateInputForJoinGame(string sessionKey, JoinGameModel model)
        {
            if (!UsersPersister.ValidateSessionKey(sessionKey))
            {
                throw new ArgumentException("Invalid format of session key");
            }

            if (model == null)
            {
                throw new ArgumentNullException("Null game model.");
            }

            if (string.IsNullOrWhiteSpace(model.GameName))
            {
                throw new ArgumentException("Invalid game name format.");
            }

            if (string.IsNullOrWhiteSpace(model.Host))
            {
                throw new ArgumentException("Invalid host name format.");
            }
        }

        private void SetPlayerAtCurrentPosition(GamesRoom room, User user)
        {
            var hubRoom = JustBelotHub.rooms.FirstOrDefault(x => x.Id == room.Id);
            if (room.Player2 == null)
            {
                room.Player2 = user.Id;
                hubRoom.Player2 = user.Id;
            }
            else if (room.Player3 == null)
            {
                room.Player3 = user.Id;
                hubRoom.Player3 = user.Id;
            }
            else if (room.Player4 == null)
            {
                room.Player4 = user.Id;
                hubRoom.Player4 = user.Id;
            }
            else
            {
                throw new ArgumentException("Room is full.");
            }
        }
    }
}