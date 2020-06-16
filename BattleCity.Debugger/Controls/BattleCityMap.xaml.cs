using BattleCity.Client.Models;
using System.Windows.Controls;

namespace BattleCity.Debugger.Controls
{
    public partial class BattleCityMap : UserControl
    {
        public BattleCityMap()
        {
            InitializeComponent();
            CreateLayerCheckBoxes();
        }

        public void SetGameState(GameState GameState)
        {
            renderer.GameState = GameState;
            renderer.InvalidateVisual();
        }

        private void CreateLayerCheckBoxes()
        {
            foreach(var layer in renderer.BattleCityLayers)
            {
                var layerCheckBox = new CheckBox();
                layerCheckBox.Content = layer.Name;
                layerCheckBox.IsChecked = layer.Enabled;
                layerCheckBox.Click += (a, b) =>
                {
                    layer.Enabled = layerCheckBox.IsChecked == true;
                    renderer.InvalidateVisual();
                };
                layers.Children.Add(layerCheckBox);
            }
        }
    }
}
