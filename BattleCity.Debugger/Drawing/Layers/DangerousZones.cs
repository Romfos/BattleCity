using BattleCity.Framework;
using System.Linq;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing.Layers
{
    public class DangerousZones : BattleCityLayer
    {
        private readonly SolidColorBrush dangerous = new SolidColorBrush(Color.FromArgb(128, 255, 0, 0));
        private readonly SolidColorBrush safe = new SolidColorBrush(Color.FromArgb(128, 255, 255, 255));

        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Dangerous zones";

        public override void Render(BattleCityDrawingContext context)
        {
            var predicates = new Predicates(context.GameState);

            DrawPositionColor(context, predicates, context.GameState.PlayerTank);

            foreach (var position in Vector.Around(context.GameState.PlayerTank).Where(x => predicates.IsVisible(x)))
            {
                DrawPositionColor(context, predicates, position);
            }
        }

        private void DrawPositionColor(BattleCityDrawingContext context, Predicates predicates, Vector position)
        {
            if (predicates.IsUnderThreat(position))
            {
                context.DrawTile(dangerous, null, position.X, position.Y, null);
            }
            else
            {
                context.DrawTile(safe, null, position.X, position.Y, null);
            }
        }
    }
}
