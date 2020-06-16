namespace BattleCity.Debugger.Drawing.Layers
{
    public class Player : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Player";

        public override void Render(BattleCityDrawingContext context)
        {
            context.DrawTile(
                context.GameState.PlayerTank.Alive ? StaticResources.Player : StaticResources.Dead, 
                null, 
                context.GameState.PlayerTank.X, 
                context.GameState.PlayerTank.Y, 
                context.GameState.PlayerTank.Direction);
        }
    }
}
