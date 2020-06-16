namespace BattleCity.Debugger.Drawing.Layers
{
    public class AITanks : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "AI tanks";

        public override void Render(BattleCityDrawingContext context)
        {
            foreach (var tank in context.GameState.AiTanks)
            {
                context.DrawTile(
                    tank.Alive ? StaticResources.AITank : StaticResources.Dead, 
                    null, 
                    tank.X, 
                    tank.Y, 
                    tank.Direction);
            }
        }
    }
}
