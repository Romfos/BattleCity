using BattleCity.Client.Models;
using Newtonsoft.Json;
using System;
using System.Text;
using System.Threading.Tasks;
using WebSocket4Net;

namespace BattleCity.Client
{
    public class BattleCityClient
    {
        private readonly TaskCompletionSource<bool> taskCompletionSource;
        private readonly WebSocket socket;
        private readonly IBattleCityRobot battleCityRobot;

        public BattleCityClient(string conenctionString, IBattleCityRobot robot)
        {
            battleCityRobot = robot;
            socket = CreateWebSocket(conenctionString);
            taskCompletionSource = new TaskCompletionSource<bool>();
        }

        private WebSocket CreateWebSocket(string connectionString)
        {
            var serverUrl = connectionString
                .Replace("http", "ws")
                .Replace("board/player/", "ws?user=")
                .Replace("?code=", "&code=");

            var socket = new WebSocket(serverUrl);
            socket.MessageReceived += (s, e) => { OnMessageReceivedHandler(e.Message); };
            socket.Closed += (s, e) => { SocketNotConnected(); };
            return socket;
        }

        public async Task ConnectAsync()
        {
            await socket.OpenAsync();
        }

        public async Task WaitAsync()
        {
            await taskCompletionSource.Task;
        }

        private void OnMessageReceivedHandler(string gameStateMessage)
        {
            try
            {
                var gameState = GetGameState(gameStateMessage);
                var robotState = battleCityRobot.GetRobotState(gameState);
                var robotStateMessage = GetRobotStateMessage(robotState);
                SendMessage(robotStateMessage);
            }
            catch(Exception ex)
            {
                taskCompletionSource.SetException(ex);
            }
        }

        private string GetRobotStateMessage(RobotState robotState)
        {
            var response = new StringBuilder();

            if (robotState.Fire == Fire.FIRE_BEFORE_ACTION)
            {
                response.Append("ACT");
                if (robotState.Command != 0)
                {
                    response.Append(",");
                    response.Append(ChooseCommand(robotState.Command));
                }
            }

            else if (robotState.Fire == Fire.FIRE_AFTER_ACTION)
            {
                response.Append(ChooseCommand(robotState.Command));
                response.Append(",");
                response.Append("ACT");
            }
            else
            {
                response.Append(ChooseCommand(robotState.Command));
            }

            return response.ToString();
        }

        private string ChooseCommand(Commands command)
        {
            return command switch
            {
                Commands.GO_TOP => "UP",
                Commands.GO_DOWN => "DOWN",
                Commands.GO_LEFT => "LEFT",
                Commands.GO_RIGHT => "RIGHT",
                _ => string.Empty,
            };
        }

        private GameState GetGameState(string message)
        {
            return JsonConvert.DeserializeObject<GameState>(message.Substring(6));
        }

        private void SendMessage(string message)
        {
            socket.Send(message);
        }

        private void SocketNotConnected()
        {
            taskCompletionSource.SetException(new Exception("Unable to connect or connection was unexpectedly lost"));
        }
    }
}
