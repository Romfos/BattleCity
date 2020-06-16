namespace BattleCity.Debugger.Drawing.Layers
{
    public class Borders : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;

        public override string Name { get; } = "Borders";

        public override void Render(BattleCityDrawingContext context)
        {
            foreach(var border in context.GameState.Borders)
            {
                context.DrawTile(StaticResources.Border, null, border.X, border.Y, null);
            }
        }
    }
}
