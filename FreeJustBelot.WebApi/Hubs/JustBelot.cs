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
    public class JustBelotHub : Hub
    {
        public static List<Game> games = new List<Game>();
        public static List<GamesRoom> rooms = new List<GamesRoom>();

        public void JoinGame(string sessionKey,string gameName)
        {
            Groups.Add(Context.ConnectionId, gameName);
            Clients.OthersInGroup(gameName).joinGame();
        }
    }
}