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
            return Dispatcher.Invoke(() => HandleGameTick(gameState));
        }

        private RobotState HandleGameTick(GameState gameState)
        {
            var robotState = new RobotState();
            if (runtime.IsChecked == true)
            {
                robotState = battleCityRobot.GetRobotState(gameState);
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

                if(!gameState.PlayerTank.Alive)
                {
                    if(history.Children.Count > 6)
                    {
                        history.Children.RemoveRange(0,  history.Children.Count - 6);
                    }
                    record.IsChecked = false;
                }
            }
            return robotState;
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
