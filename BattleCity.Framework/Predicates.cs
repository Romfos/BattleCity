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

        public bool IsUnderBulletThreat(Vector position) => gameState.Bullets
            .Any(x => position == Vector.FromDirection(x.Direction) + x
                || position == Vector.FromDirection(x.Direction) * 2 + x);

        public bool IsUnderEnemyTankThreat(Vector position) => gameState.Enemies.Concat(gameState.AiTanks)
            .Where(x => new[] { 1, 2, 4 }.Contains(x.FireCoolDown))
            .SelectMany(tank => Vector.Directions.Select(direction => tank + direction)
                .Concat(Vector.Directions.Select(direction => tank + direction * 2)))
                    .Any(x => position == x);

        public bool IsUnderThreat(Vector position) => IsUnderBulletThreat(position) || IsUnderEnemyTankThreat(position);
    }
}
