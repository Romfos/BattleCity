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
                ?? FindEnemy(gameState, predicates, navigation)
                ?? Battle(gameState, predicates, navigation);
        }

        private RobotState Battle(GameState gameState, Predicates predicates, Navigation navigation)
        {
            var robotState = new RobotState
            {
                Command = predicates.GetTheMostSafeDirection().ToCommand()
            };

            if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, Vector.FromDirection(gameState.PlayerTank.Direction)))
            {
                robotState.Fire = Fire.FIRE_BEFORE_ACTION;
            }

            if(predicates.IsReadyToFire)
            {
                var direction = navigation.GetTargetDirection();
                if (!predicates.IsUnderThreat(gameState.PlayerTank + direction))
                {
                    robotState.Command = direction.ToCommand();
                    if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, direction))
                    {
                        robotState.Fire = Fire.FIRE_AFTER_ACTION;
                    }
                }                
            }

            return robotState;
        }

        private RobotState FindEnemy(GameState gameState, Predicates predicates, Navigation navigation)
        {
            if (navigation.GetStepCountToTarget() > 5)
            {
                var direction = navigation.GetTargetDirection();

                var robotState = new RobotState
                {
                    Command = direction.ToCommand()
                };

                if (predicates.IsUnderBulletThreat(gameState.PlayerTank + direction))
                {
                    robotState.Command = predicates.GetTheMostSafeDirection().ToCommand();
                }

                return robotState;
            }
            return null;
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
