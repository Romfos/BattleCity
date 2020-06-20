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

            return HandleDeadState(gameState) 
                ?? HandleBlockState(navigation)
                ?? HandleFarEnemyState(navigation)
                ?? HandleBattleState()
                ?? new RobotState();
        }

        private RobotState HandleBattleState()
        {
            return new RobotState();
        }

        private RobotState HandleFarEnemyState(Navigation navigation)
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

        private RobotState HandleBlockState(Navigation navigation)
        {
            if(!navigation.GoToTargetDirection().HasValue)
            {
                return new RobotState { Fire = Fire.FIRE_BEFORE_ACTION };
            }
            return null;
        }

        private RobotState HandleDeadState(GameState gameState)
        {
            if (!gameState.PlayerTank.Alive)
            {
                return new RobotState();
            }
            return null;
        }
    }
}
