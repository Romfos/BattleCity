using BattleCity.Client.Models;

namespace BattleCity.Client
{
    public interface IBattleCityRobot
    {
        RobotState GetRobotState(GameState gameState);
    }
}
