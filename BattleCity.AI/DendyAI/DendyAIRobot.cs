using BattleCity.Client;
using BattleCity.Client.Models;
using BattleCity.Framework;

namespace BattleCity.AI.DendyAI
{
    public class DendyAIRobot : IBattleCityRobot
    {
        public RobotState GetRobotState(GameState gameState)
        {
            var predicates = new Predicates(gameState);
            var navigation = new Navigation(gameState, predicates);

            return Dead(gameState)
                ?? BadRespawn(navigation)
                ?? FindEnemy(navigation)
                ?? Battle()
                ?? NoAction();
        }

        private RobotState Battle()
        {
            return new RobotState();
        }

        private RobotState FindEnemy(Navigation navigation)
        {
            if (navigation.GetStepCountToTarget() > 7)
            {
                return new RobotState
                {
                    Command = navigation.GoToTargetDirection().Value.ToCommand()
                };
            }
            return null;
        }

        private RobotState BadRespawn(Navigation navigation)
        {
            if(!navigation.GoToTargetDirection().HasValue)
            {
                return new RobotState { Fire = Fire.FIRE_AFTER_ACTION, Command = Commands.GO_TOP };
            }
            return null;
        }

        private RobotState Dead(GameState gameState)
        {
            if (!gameState.PlayerTank.Alive)
            {
                return new RobotState();
            }
            return null;
        }

        private RobotState NoAction()
        {
            return new RobotState();
        }
    }
}
