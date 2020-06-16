namespace BattleCity.Debugger.Drawing.Layers
{
    public class Background : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;

        public override string Name { get; } = "Background";

        public override void Render(BattleCityDrawingContext context)
        {
            context.Clear();
        }
    }
}
