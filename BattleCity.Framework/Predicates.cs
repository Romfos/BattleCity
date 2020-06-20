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
        public bool IsEnemy(Vector position) => gameState.Enemies.Any(x => x == position && x.Alive);
        public bool IsAiTank(Vector position) => gameState.AiTanks.Any(x => x == position && x.Alive);
        public bool IsBorder(Vector position) => gameState.Borders.Any(x => x == position);
        public bool IsBullet(Vector position) => gameState.Bullets.Any(x => x == position);
        public bool IsConstruction(Vector position) => gameState.Constructions.Any(x => x == position);

        // complex
        public bool IsEnemyTank(Vector position) => IsEnemy(position) || IsAiTank(position);
        public bool IsVisible(Vector position) => !IsBorder(position) && !IsConstruction(position);
        public bool IsFree(Vector position) => IsVisible(position) && !IsEnemyTank(position);
    }
}
