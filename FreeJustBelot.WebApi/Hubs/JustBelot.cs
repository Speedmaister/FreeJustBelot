using FreeJustBelot.Data;
using FreeJustBelot.DataLayer;
using FreeJustBelot.Models;
using FreeJustBelot.WebApi.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FreeJustBelot.WebApi.Hubs
{
    
    public class JustBelotWaitRoom : Hub
    {
        private static FreeJustBelotEntities context = new FreeJustBelotEntities();
        private static IRepository<Game> gamesRepository = new Repository<Game>(context);
        private static IRepository<User> usersRepository = new Repository<User>(context);

        public void JoinRoom(string gameName)
        {
            Groups.Add(Context.ConnectionId, gameName);
            var room = GetRoomModel(gameName);
            Clients.Group(gameName).PlayerJoinedRoom(room);
        }

        public void LeaveRoom(string gameName,string playerName)
        {
            Clients.OthersInGroup(gameName).PlayerLeftRoom(playerName);
        }

        private static RoomModel GetRoomModel(string gameName)
        {
            var room = gamesRepository.All().FirstOrDefault(x => x.Name == gameName);
            string user1 = usersRepository.Get((int)room.Player1).Nickname;
            string user2 = null;
            string user3 = null;
            string user4 = null;
            if (room.Player2 != null)
            {
                user2 = usersRepository.Get((int)room.Player2).Nickname;
            }

            if (room.Player3 != null)
            {
                user3 = usersRepository.Get((int)room.Player3).Nickname;
            }

            if (room.Player4 != null)
            {
                user4 = usersRepository.Get((int)room.Player4).Nickname;
            }

            RoomModel roomModel = new RoomModel();
            roomModel.Players.Add(user1);
            roomModel.Players.Add(user2);
            roomModel.Players.Add(user3);
            roomModel.Players.Add(user4);

            return roomModel;
        }
    }
}