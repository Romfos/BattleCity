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
                ?? Battle(gameState, predicates, navigation);
        }

        private RobotState Battle(GameState gameState, Predicates predicates, Navigation navigation)
        {
            var robotState = new RobotState();

            if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, Vector.FromDirection(gameState.PlayerTank.Direction)))
            {
                robotState.Fire = Fire.FIRE_BEFORE_ACTION;
            }

            var direction = GetBattleMoveDirection(gameState, predicates, navigation);
            if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, direction))
            {
                robotState.Fire = Fire.FIRE_AFTER_ACTION;
            }

            robotState.Command = direction.ToCommand();

            return robotState;
        }

        private Vector GetBattleMoveDirection(GameState gameState, Predicates predicates, Navigation navigation)
        {
            var direction = navigation.GetTargetDirection();            

            if(!predicates.IsUnderThreat(gameState.PlayerTank + direction))
            {
                return direction;
            }

            var safedirection = predicates.GetTheMostSafeDirection();

            if (predicates.IsUnderThreat(gameState.PlayerTank + safedirection))
            {
                return direction;
            }
            
            return safedirection;
        }

        private RobotState BadRespawn(Navigation navigation)
        {
            if(!navigation.GetStepCountToTarget().HasValue)
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
    }
}
