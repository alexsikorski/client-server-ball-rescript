using System;
using System.IO;
using System.Net.Sockets;
using System.Threading;
using BallGameClient.Utilities;

namespace BallGameClient
{
    public class Client
    {
        public static void Main(String[] args)
        {
            // starting _client thread
            Thread clientThread = new Thread(ClientThread.RunClient);
            clientThread.Start();
        }
    }

    class ClientThread
    {
        private static StreamWriter _dos;
        private static StreamReader _dis;

        public static void RunClient()
        {
            // connect to tcp listener
            TcpClient _client = new TcpClient("localhost", Constants.PortNumber);
            // setting streams
            
            using (Stream stream = _client.GetStream())
            {
                _dis = new StreamReader(stream);
                _dos = new StreamWriter(stream);
                _dos.AutoFlush = true;
                while (true)
                {
                    bool hasBall; // to determine if client has ball
                    try
                    {
                    
                        string line = _dis.ReadLine();
                        if (line == "has_ball")
                        {
                            hasBall = true;
                            Console.WriteLine("YOU HAVE THE BALL!!! Who would you like to pass it to? If you would like to refresh player list, enter 0.");
                            string toSend = Console.ReadLine();
                            _dos.WriteLine(toSend);
                            _dos.Flush();
                        }
                        else
                        {
                            Console.WriteLine(line); // displays players
                        }

                    }
                    catch (IOException e)
                    {
                        Console.WriteLine(e);
                        throw;
                    }

                }
            }
       
        }
    }
}
