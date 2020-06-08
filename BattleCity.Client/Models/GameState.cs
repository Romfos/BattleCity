using System.Collections.Generic;

namespace BattleCity.Client.Models
{
    public class GameState
    {
        public Tank PlayerTank { get; set; }
        public List<Tank> AiTanks { get; set; }
        public List<Tank> Enemies { get; set; }
        public List<Construction> Constructions { get; set; }
        public List<Border> Borders { get; set; }
        public List<Bullet> Bullets { get; set; }
    }
}