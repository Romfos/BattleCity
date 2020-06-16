namespace BattleCity.Debugger.Drawing.Layers
{
    public class Constructions : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Constructions";

        public override void Render(BattleCityDrawingContext context)
        {
            foreach (var border in context.GameState.Constructions)
            {
                context.DrawTile(StaticResources.Construction, null, border.X, border.Y, null);
            }
        }
    }
}
