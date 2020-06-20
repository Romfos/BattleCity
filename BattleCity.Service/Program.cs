using BattleCity.AI.DendyAI;
using BattleCity.Client;
using System;
using System.Threading.Tasks;

namespace BattleCity.Service
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var connectionString = Environment.GetEnvironmentVariable("url");
            if(string.IsNullOrEmpty(connectionString))
            {
                connectionString = "http://battlecity.godeltech.com/codenjoy-contest/board/player/bcxjamqy6zk30w5o4up4?code=4120728259014700513&gameName=battlecity";
            }

            var robot = new DendyAIRobot();

            var battleCityClient = new BattleCityClient(connectionString, robot);
            await battleCityClient.ConnectAsync();
            await battleCityClient.WaitAsync();
        }
    }
}
