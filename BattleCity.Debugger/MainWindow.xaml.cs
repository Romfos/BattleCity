using BattleCity.AI.DendyAI;
using BattleCity.Client;
using BattleCity.Client.Models;
using System;
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

            var connectionString = Environment.GetEnvironmentVariable("url");
            if (string.IsNullOrEmpty(connectionString))
            {
                connectionString = "http://battlecity.godeltech.com/codenjoy-contest/board/player/bcxjamqy6zk30w5o4up4?code=4120728259014700513&gameName=battlecity";
            }

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
                var command = robotState.Command switch
                {
                    Commands.GO_LEFT => "Left",
                    Commands.GO_TOP => "Top",
                    Commands.GO_RIGHT => "Right",
                    Commands.GO_DOWN => "Down",
                    _ => "None"
                };
                var tempalte = robotState.Fire switch
                {
                    Fire.FIRE_BEFORE_ACTION => "Fire + {0}",
                    Fire.FIRE_AFTER_ACTION => "{0} + Fire",
                    _ => "{0}",
                };
                historyButton.Content = string.Format(tempalte, command);

                historyButton.HorizontalContentAlignment = HorizontalAlignment.Left;
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
