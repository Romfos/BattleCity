namespace BattleCity.Client.Models
{
    public class Tank : Point
    {
        public string Direction { get; set; }
        public int FireCoolDown { get; set; }
        public int KilledCounter { get; set; }
        public bool Alive { get; set; }
    }
}