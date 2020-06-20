using System.Linq;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing.Layers
{
    public class Cooldowns : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Fire cooldowns";

        public override void Render(BattleCityDrawingContext context)
        {
            var brush = Brushes.Cyan;

            context.DrawTileText(
                context.GameState.PlayerTank.FireCoolDown.ToString(),
                20,
                brush,
                null,
                context.GameState.PlayerTank.X,
                context.GameState.PlayerTank.Y);

            foreach(var tank in context.GameState.AiTanks.Where(x => x.Alive))
            {
                context.DrawTileText(
                    tank.FireCoolDown.ToString(),
                    20,
                    brush,
                    null,
                    tank.X,
                    tank.Y);
            }

            foreach (var tank in context.GameState.Enemies.Where(x => x.Alive))
            {
                context.DrawTileText(
                    tank.FireCoolDown.ToString(),
                    20,
                    brush,
                    null,
                    tank.X,
                    tank.Y);
            }
        }
    }
}
