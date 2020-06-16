using BattleCity.Client;
using BattleCity.Client.Models;
using System.Windows;
using System.Windows.Controls;

namespace BattleCity.Debugger
{
    public partial class MainWindow : Window, IBattleCityRobot
    {
        private readonly BattleCityClient battleCityClient;
        private readonly IBattleCityRobot battleCityRobot;

        public MainWindow()
        {
            InitializeComponent();

            var connectionString = "http://battlecity.godeltech.com/codenjoy-contest/board/player/bcxjamqy6zk30w5o4up4?code=4120728259014700513&gameName=battlecity";
            battleCityClient = new BattleCityClient(connectionString, this);
        }

        public RobotState GetRobotState(GameState gameState)
        {
            Dispatcher.Invoke(() =>
            {
                if (runtime.IsChecked == true)
                {
                    map.SetGameState(gameState);
                }
                if(record.IsChecked == true)
                {
                    var historyButton = new Button();
                    historyButton.Content = history.Children.Count + 1;
                    historyButton.Click += (a, b) =>
                    {
                        runtime.IsChecked = false;
                        record.IsChecked = false;
                        map.SetGameState(gameState);
                    };
                    history.Children.Add(historyButton);
                }
            });
            return new RobotState();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await battleCityClient.ConnectAsync();
        }

        private void clear_Click(object sender, RoutedEventArgs e)
        {
            history.Children.Clear();
        }
    }
}
