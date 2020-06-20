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
                ?? TooFarFromEnemy(gameState, predicates, navigation)
                ?? NotReadyToBattle(predicates)
                ?? ReadyToBattle(gameState, predicates, navigation);
        }

        private RobotState ReadyToBattle(GameState gameState, Predicates predicates, Navigation navigation)
        {
            var robotState = new RobotState();


            if (predicates.IsUnderBulletThreat(gameState.PlayerTank))
            {
                robotState.Command = predicates.GoToTheMostSafeDirection().ToCommand();
            }
            else
            {
                if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, Vector.FromDirection(gameState.PlayerTank.Direction)))
                {
                    robotState.Fire = Fire.FIRE_BEFORE_ACTION;
                    robotState.Command = predicates.GoToTheMostSafeDirection().ToCommand(); ;
                }

                var direction = navigation.GetTargetDirection();                
                if (predicates.IsUnderThreat(gameState.PlayerTank + direction))
                {
                    direction = predicates.GoToTheMostSafeDirection();
                }
                robotState.Command = direction.ToCommand();

                if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, direction))
                {
                    robotState.Fire = Fire.FIRE_AFTER_ACTION;
                }
            }

            return robotState;
        }

        private RobotState NotReadyToBattle(Predicates predicates)
        {
            if (!predicates.IsReadyToFire)
            {
                return new RobotState
                {
                    Command = predicates.GoToTheMostSafeDirection().ToCommand()
                };
            }

            return null;
        }

        private RobotState TooFarFromEnemy(GameState gameState, Predicates predicates, Navigation navigation)
        {
            if (navigation.GetStepCountToTarget() > 5)
            {
                var direction = navigation.GetTargetDirection();
                if (predicates.IsUnderBulletThreat(gameState.PlayerTank) 
                    || predicates.IsUnderBulletThreat(gameState.PlayerTank + direction))
                {
                    return new RobotState
                    {
                        Command = predicates.GoToTheMostSafeDirection().ToCommand()
                    };
                }
                else
                {
                    return new RobotState
                    {
                        Command = direction.ToCommand()
                    };
                }
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
