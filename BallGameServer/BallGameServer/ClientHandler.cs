using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Timers;
using BallGameServer.Utilities;

namespace BallGameServer
{

    public class ClientHandler
    {
        private StreamWriter dos;
        private StreamReader dis;
        public int ClientId;
        private Stream ClientStream;
        private TcpClient Client;
        


        System.Timers.Timer timer = new System.Timers.Timer();
        Ball ball = new Ball();

        public ClientHandler (Stream clientStream, int clientId, TcpClient client)
        {
            this.ClientStream = clientStream;
            this.ClientId = clientId;
            this.Client = client;
           

            dos = new StreamWriter(ClientStream);
            dis = new StreamReader(ClientStream);
            dos.AutoFlush = true;
        }


        public void ClientThread()
        {
            
            SetTimer(timer); // starts timer
            
        }

        public void SetTimer(System.Timers.Timer timer)
        {
            
            timer.Elapsed+=new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = Constants.Delay;
            timer.Enabled = true;
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            
            string received;
            int receivedInt;
            try
            {
                try
                {
                    
                    dos.WriteLine("You are Player: " + ClientId + "\n" + "CONNECTED PLAYERS: \n" + Server.StringPlayers() + "\n" + "Player: " + ball.WhoHasBall() + " has the ball!\n");
                    
                    // does this client have the ball?
                    if (ball.DoesClientHaveBall(ClientId)) // if this client does (true)
                    {
                        dos.WriteLine("has_ball");
                    }

                    received = dis.ReadLine();
                    receivedInt = Int32.Parse(received);
                    if (receivedInt > 0)
                    {
                        ball.GiveBall(receivedInt, ClientId);
                    }

                }
                catch (IOException)
                {
                    Console.WriteLine("Player: " + ClientId + ", " + " Connection closed.");
                    // remove by value from ConnectedClients
                    Server.ConnectedClients.Remove(Client);
                    // ball
                    ball.RemoveBall(ClientId);
                    Server.GameState.Remove(ClientId);
                    ball.DisconnectBall();

                    Console.WriteLine();
                    Console.WriteLine("CONNECTED PLAYERS");
                    if (String.IsNullOrEmpty(Server.StringPlayers()))
                    {
                        Console.WriteLine("No players.");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine(Server.StringPlayers());
                    }
                    ClientStream.Close();
                    Client.Close(); // closes client

                    timer.Stop();
                    timer.Close();
                }
                
            }
            catch (SocketException)
            {

            }
        }
    }
}