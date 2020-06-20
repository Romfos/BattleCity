using BattleCity.Framework;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing.Layers
{
    public class NavigationMap : BattleCityLayer
    {
        public override bool Enabled { get; set; } = false;
        public override string Name { get; } = "Navigation map";

        public override void Render(BattleCityDrawingContext context)
        {
            var predicates = new Predicates(context.GameState);
            var navigation = new Navigation(context.GameState, predicates);

            foreach (var steps in navigation.Map)
            {
                if(steps.Value.Count < 7)
                {
                    context.DrawTileText(
                        steps.Value.Count.ToString(),
                        16,
                        Brushes.Red,
                        new Pen(Brushes.Gray, 1),
                        steps.Key.X,
                        steps.Key.Y);
                }
            }
        }
    }
}
