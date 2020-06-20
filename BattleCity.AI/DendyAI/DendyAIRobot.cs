using BattleCity.Client;
using BattleCity.Client.Models;

namespace BattleCity.AI.DendyAI
{
    public class DendyAIRobot : IBattleCityRobot
    {
        public RobotState GetRobotState(GameState gameState)
        {
            return new RobotState();
        }
    }
}
