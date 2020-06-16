namespace BattleCity.Debugger.Drawing.Layers
{
    public class Enemies : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Enemies";

        public override void Render(BattleCityDrawingContext context)
        {
            foreach (var tank in context.GameState.Enemies)
            {
                context.DrawTile(
                    tank.Alive ? StaticResources.Enemy : StaticResources.Dead, 
                    null, 
                    tank.X, 
                    tank.Y, 
                    tank.Direction);
            }
        }
    }
}
;