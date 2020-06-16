namespace BattleCity.Debugger.Drawing.Layers
{
    public abstract class BattleCityLayer
    {
        public abstract bool Enabled { get; set; }

        public abstract string Name { get; }

        public abstract void Render(BattleCityDrawingContext context);
    }
}
