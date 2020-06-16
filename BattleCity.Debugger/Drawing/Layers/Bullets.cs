namespace BattleCity.Debugger.Drawing.Layers
{
    public class Bullets : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Bullets";

        public override void Render(BattleCityDrawingContext context)
        {
            foreach (var bullet in context.GameState.Bullets)
            {
                context.DrawTile(StaticResources.Bullet, null, bullet.X, bullet.Y, null);
            }
        }
    }
}
