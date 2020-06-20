using BattleCity.Client.Models;
using BattleCity.Debugger.Drawing;
using BattleCity.Debugger.Drawing.Layers;
using System.Windows.Controls;
using System.Windows.Media;

namespace BattleCity.Debugger.Controls
{
    public partial class BattleCityRenderer : UserControl
    {
        public GameState GameState { get; set; }
        public BattleCityLayer[] BattleCityLayers { get; } =
        {
            new Background(),
            new Borders(),
            new Constructions(),
            new AITanks(),
            new Enemies(),
            new Player(),
            new Bullets(),
            new Cooldowns(),
            new NavigationMap()
        };

        public BattleCityRenderer()
        {
            InitializeComponent();
        }

        protected override void OnRender(DrawingContext drawingContext)
        {
            if (GameState != null)
            {
                var conbattleCityDrawingContextext = PrepareBattleCityDrawingContext(drawingContext);
                RenderLayers(conbattleCityDrawingContextext);
            }
        }

        private BattleCityDrawingContext PrepareBattleCityDrawingContext(DrawingContext drawingContext)
        {
            var context = new BattleCityDrawingContext(GameState, drawingContext);
            Width = context.RenderSize.Width;
            Height = context.RenderSize.Height;
            return context;
        }

        private void RenderLayers(BattleCityDrawingContext battleCityDrawingContext)
        {
            foreach (var battleCityLayer in BattleCityLayers)
            {
                if (battleCityLayer.Enabled)
                {
                    battleCityLayer.Render(battleCityDrawingContext);
                }
            }
        }
    }
}
