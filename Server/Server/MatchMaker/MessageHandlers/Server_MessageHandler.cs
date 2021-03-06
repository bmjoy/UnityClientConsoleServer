﻿using MatchMaker;
using Server;
using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Class that is responsible for getting a serialized object 
/// and giving it to the correct handler
/// </summary>
public class Server_MessageHandler :IMessageHandler {
    private MatchMakerCore matchMakerCore;
    private ServerCore server;
    private MessageCommandHandler commandHandler;

    public void Setup(ServerCore server, MatchMakerCore matchMakerCore)
    {
        this.matchMakerCore = matchMakerCore;
        this.server = server;
        commandHandler = new MessageCommandHandler();
        commandHandler.Add(typeof(Message_Request_JoinQueue), new MessageHandler_Request_JoinQueue(server, matchMakerCore));
        commandHandler.Add(typeof(Message_Request_LeaveQueue), new MessageHandler_Request_LeaveQueue(server.clientManager, matchMakerCore));
        commandHandler.Add(typeof(Message_ClientResponse_ReadyCheck), new MessageHandler_Response_ReadyCheck(matchMakerCore, server));
    }
    /// <summary>
    /// The method responsible for getting a serialized object 
    /// and giving it to the correct handler
    /// </summary>
    /// <param name="data"></param>
    /// <param name="client"></param>
    public void Handle(object data, Server_ServerClient client)
    {
        Console.WriteLine("Data recieved of type " + data.ToString());
        if(commandHandler.Contains(data.GetType()))
            commandHandler.Execute(data.GetType(),data,client);
        else
        {
            Console.WriteLine("Data type UKNOWN! Type: " + data.GetType().ToString());
            throw new Exception("Data type UKNOWN! Type: " + data.GetType().ToString());
        }
    }

}