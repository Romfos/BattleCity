using BattleCity.Framework;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing.Layers
{
    public class DangerousZones : BattleCityLayer
    {
        private readonly SolidColorBrush brush = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));

        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Dangerous zones";

        public override void Render(BattleCityDrawingContext context)
        {
            var predicates = new Predicates(context.GameState);

            if (predicates.IsUnderThreat(context.GameState.PlayerTank))
            {
                context.DrawTile(brush, null, context.GameState.PlayerTank.X, context.GameState.PlayerTank.Y, null);
            }

            foreach (var position in Vector.Around(context.GameState.PlayerTank))
            {
                if(predicates.IsUnderThreat(position))
                {
                    context.DrawTile(brush, null, position.X, position.Y, null);
                }
            }
        }
    }
}
