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
                ?? Battle(gameState, predicates, navigation);
        }

        private RobotState Battle(GameState gameState, Predicates predicates, Navigation navigation)
        {
            var robotState = new RobotState();

            var theMostSafePosition = predicates.GetTheMostSafePosition();
            var goToTheMostSafePosition = (theMostSafePosition - gameState.PlayerTank).ToCommand();

            if (!predicates.IsReadyToFire)
            {
                if(predicates.IsUnderThreat(gameState.PlayerTank))
                {
                    robotState.Command = goToTheMostSafePosition;
                }
                else
                {
                    // if safe and not ready - do nothing
                    return robotState;
                }
            }
            else
            {
                if(predicates.HasEnemyTankOnRay(gameState.PlayerTank, Vector.FromDirection(gameState.PlayerTank.Direction)))
                {
                    robotState.Fire = Fire.FIRE_BEFORE_ACTION;
                    robotState.Command = goToTheMostSafePosition;
                }
                else
                {
                    var direction = navigation.GoToTargetDirection().Value;

                    if(predicates.IsUnderBulletThreat(gameState.PlayerTank) 
                        || predicates.IsUnderBulletThreat(gameState.PlayerTank + direction))
                    {
                        robotState.Command = goToTheMostSafePosition;
                    }
                    else
                    {
                        robotState.Command = direction.ToCommand();
                    }

                    if (predicates.HasEnemyTankOnRay(gameState.PlayerTank, direction))
                    {
                        robotState.Fire = Fire.FIRE_AFTER_ACTION;
                    }
                }
            }

            return robotState;
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
    }
}
