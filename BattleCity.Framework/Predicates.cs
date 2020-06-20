using BattleCity.Client.Models;
using System.Linq;

namespace BattleCity.Framework
{
    public class Predicates
    {
        private readonly GameState gameState;

        public Predicates(GameState gameState)
        {
            this.gameState = gameState;
        }

        public bool IsReadyToFire => gameState.PlayerTank.FireCoolDown < 2;

        // basic
        public bool IsPlayer(Vector position) => position == gameState.PlayerTank;
        public bool IsEnemyTank(Vector position) => gameState.Enemies.Any(x => x == position && x.Alive);
        public bool IsAiTank(Vector position) => gameState.AiTanks.Any(x => x == position && x.Alive);
        public bool IsBorder(Vector position) => gameState.Borders.Any(x => x == position);
        public bool IsBullet(Vector position) => gameState.Bullets.Any(x => x == position);
        public bool IsConstruction(Vector position) => gameState.Constructions.Any(x => x == position);

        // complex
        public bool IsEnemyOrAiTank(Vector position) => IsEnemyTank(position) || IsAiTank(position);
        public bool IsVisible(Vector position) => !IsBorder(position) && !IsConstruction(position);
        public bool IsFree(Vector position) => IsVisible(position) && !IsEnemyTank(position);

        public bool HasEnemyTankOnRay(Vector position, Vector direction) =>
            Vector.Ray(position, direction).TakeWhile(IsVisible).ToList().Any(IsEnemyOrAiTank);

        public bool IsUnderBulletThreat(Vector position) => gameState.Bullets
            .Any(x => position == x 
                || position == Vector.FromDirection(x.Direction) + x
                || position == Vector.FromDirection(x.Direction) * 2 + x
                || position == Vector.FromDirection(x.Direction) * 3 + x);

        public bool IsUnderAiTankThreat(Vector position) => gameState.AiTanks
            .Where(x => x.FireCoolDown < 2)
            .SelectMany(tank => Vector.Directions.Select(direction => tank + direction)
                .Concat(Vector.Directions.Select(direction => tank + direction * 2))
                .Concat(Vector.Directions.Select(direction => tank + direction * 3)))
                    .Any(x => position == x);

        public bool IsUnderEnemyTankThreat(Vector position) => gameState.Enemies
            .Where(x => x.FireCoolDown < 2)
            .SelectMany(tank => Vector.Directions.Select(direction => tank + direction)
                .Concat(Vector.Directions.Select(direction => tank + direction * 2))
                .Concat(Vector.Directions.Select(direction => tank + direction * 3)))
                    .Any(x => position == x);

        public bool IsUnderThreat(Vector position) => IsUnderBulletThreat(position) 
            || IsUnderEnemyTankThreat(position) 
            || IsUnderAiTankThreat(position);

        public Vector GetTheMostSafeDirection() => Vector.Around(gameState.PlayerTank)
            .Concat(new Vector[] { gameState.PlayerTank })
            .Where(IsFree)
            .OrderBy(x => IsUnderBulletThreat(x) ? 3
                : IsUnderEnemyTankThreat(x) ? 2
                : IsUnderAiTankThreat(x) ? 1
                : 0)
            .Select(x => x - gameState.PlayerTank)
            .First();
    }
}
