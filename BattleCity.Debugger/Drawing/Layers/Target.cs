using BattleCity.Framework;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing.Layers
{
    public class Target : BattleCityLayer
    {
        public override bool Enabled { get; set; } = true;
        public override string Name { get; } = "Target";

        public override void Render(BattleCityDrawingContext context)
        {
            var predicates = new Predicates(context.GameState);
            var navigation = new Navigation(context.GameState, predicates);

            if(navigation.Target == null)
            {
                return;
            }

            foreach(var step in navigation.Target.Value.Value)
            {
                context.DrawTile(null, new Pen(Brushes.Red, 1), step.X, step.Y, null);
            }
        }
    }
}
