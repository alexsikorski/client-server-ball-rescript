using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using BallGameServer.Utilities;


namespace BallGameServer
{
    class Server
    {
        public static Dictionary<TcpClient, int> ConnectedClients = new Dictionary<TcpClient, int>();
        public static SortedDictionary<int, bool> GameState = new SortedDictionary<int, bool>();


        public static TcpListener server;
        private static int _clientId = 0;

        public static String StringPlayers()
        {
            String str = "";
            foreach (int value in ConnectedClients.Values)
            {
                str = str + "Client ID: " + value + "\n";
            }
            return str;
        }

        public static void CheckIfFirstPlayer(int clientId)
        {
            if (GameState.Count == 0)
            {
                GameState[clientId] = true;
                Console.WriteLine("First player! Ball is handed to ClientID: " + clientId);
            }
            else
            {
                GameState[clientId] = false;
            }
        }
        public static void Main(String[] args)
        {
            Console.WriteLine("Welcome. Waiting for connections...");
            try
            {
                server = new TcpListener(IPAddress.Loopback, Constants.PortNumber);

                // starts listening for client requests
                server.Start();

                // listening loop
                while (true)
                {
                    try
                    {
                        TcpClient client = server.AcceptTcpClient(); // try accept client
                        _clientId++; // increment ClientId
                        ConnectedClients.Add(client,_clientId);
                        CheckIfFirstPlayer(_clientId); // checks if first player

                        Console.WriteLine("ClientID: " + _clientId + ", " + "Connected Successfully.");
                        Console.WriteLine();
                        Console.WriteLine("CONNECTED PLAYERS: ");
                        Console.WriteLine(StringPlayers());


                        Stream stream = client.GetStream();
                        // creating instance object
                        ClientHandler clientObj = new ClientHandler(stream, _clientId, client);

                        // starting new thread using class
                        Thread thread = new Thread(new ThreadStart(clientObj.ClientThread));
                        thread.Start();

                    }
                    catch (Exception e)
                    {
                        Console.WriteLine(e);
                        server.Stop(); // closes listener
                        throw;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

    }
}
