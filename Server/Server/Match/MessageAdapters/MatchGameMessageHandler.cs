﻿using Server;
using System;
using System.Diagnostics;

namespace Match
{
    /// <summary>
    /// Handles messages from the client when they are in a match
    /// </summary>
    public class MatchGameMessageHandler : IMessageHandler
    {
        public GameEngine gameEngine;
        private ILogger logger;
        private MessageCommandHandler commandHandler;
        private IGameEngineSender sender;

        public MatchGameMessageHandler(ILogger logger,GameEngine gameEngine)
        {
            this.gameEngine = gameEngine;
            this.logger = logger;
            commandHandler = new MessageCommandHandler();
            commandHandler.Add(typeof(Message_Request_PlayCard),new MessageHandler_Request_PlayCard(gameEngine));
        }

        public void Handle(object data, Server_ServerClient client)
        {
            if (commandHandler.Contains(data.GetType()))
                commandHandler.Execute(data.GetType(), data, client);
            else
            {
                Console.WriteLine("Data type UKNOWN! Type: " + data.GetType().ToString());
                throw new Exception("Data type UKNOWN! Type: " + data.GetType().ToString());
            }
        }

        /// <summary>
        /// Setup handlers that requires a sender to reply with
        /// </summary>
        /// <param name="sender"></param>
        internal void Init(IGameEngineSender sender)
        {
            this.sender = sender;
            commandHandler.Add(typeof(Message_Request_JoinGame), new MessageHandler_Request_JoinGame(sender, logger, gameEngine));
        }
    }
}