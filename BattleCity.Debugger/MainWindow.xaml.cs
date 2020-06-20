using BattleCity.AI.DendyAI;
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
            battleCityRobot = new DendyAIRobot();
        }

        public RobotState GetRobotState(GameState gameState)
        {
            var robotState = battleCityRobot.GetRobotState(gameState);
            Dispatcher.Invoke(() => HandleGameTick(gameState, robotState));
            return robotState;
        }

        private void HandleGameTick(GameState gameState, RobotState robotState)
        {
            if (runtime.IsChecked == true)
            {
                map.SetGameState(gameState);
            }
            if (record.IsChecked == true)
            {
                var historyButton = new Button();
                historyButton.Content = $"{history.Children.Count + 1}, {robotState.Command} {robotState.Fire}";
                historyButton.Click += (a, b) =>
                {
                    runtime.IsChecked = false;
                    record.IsChecked = false;
                    map.SetGameState(gameState);
                    battleCityRobot.GetRobotState(gameState);
                };
                history.Children.Add(historyButton);
            }
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
