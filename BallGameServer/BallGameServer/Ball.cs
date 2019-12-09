using System;
using System.Collections.Generic;
using System.Linq;

namespace BallGameServer
{
    public class Ball
    {
        public void GiveBall(int received, int currentId)
        {
            // validation
            if (DoesClientExist(received) && received != currentId)
            {
                Console.WriteLine("Player: " + currentId + " passes ball to Player: " + received + ".");
                Server.GameState[received] = true;
                RemoveBall(currentId);
            }
            else if (received == currentId)
            {
                Console.WriteLine("Player: " + received + " is throwing the ball back to him/herself...");
            }
            else
            {
                Console.WriteLine("Player: " + received + " does not exist, returning ball!");
                Server.GameState[currentId] = true;
            }
        }

        public void RemoveBall(int clientId)
        {
            Server.GameState[clientId] = false;
        }

        public int WhoHasBall()
        {
            int clientId = 0;
            foreach (KeyValuePair<int, bool> keyValuePair in Server.GameState)
            {
                if (keyValuePair.Value == true)
                {
                    clientId = keyValuePair.Key;
                }
            }

            return clientId;
        }

        public bool DoesClientExist(int clientId)
        {
            bool isEquiv = false;
            foreach (int keys in Server.GameState.Keys)
            {
                if (keys == clientId)
                {
                    isEquiv = true;
                    break;
                }
            }

            return false;
        }

        public bool DoesClientHaveBall(int clientId)
        {
            int clientIdHasBall = WhoHasBall();
            if (clientIdHasBall == clientId)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void DisconnectBall()
        {
            if (Server.GameState.ContainsValue(false) && Server.GameState.Count >= 1)
            {
                int lastClientId = Server.GameState.Keys.Last();
                Console.WriteLine("Automatically passing ball to ClientID: " + lastClientId);
                Server.GameState[lastClientId] = true;
            }

            if (Server.GameState.Count == 0)
            {
                Console.WriteLine("No one to give the ball to!");
            }
        }
    }
}