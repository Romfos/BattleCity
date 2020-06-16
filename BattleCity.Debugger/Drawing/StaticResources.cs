using System;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BattleCity.Debugger.Drawing
{
    public static class StaticResources
    {
        public static readonly ImageBrush AITank = new ImageBrush(new BitmapImage(new Uri("images/ai.png", UriKind.Relative)));
        public static readonly ImageBrush Border = new ImageBrush(new BitmapImage(new Uri("images/border.png", UriKind.Relative)));
        public static readonly ImageBrush Player = new ImageBrush(new BitmapImage(new Uri("images/player.png", UriKind.Relative)));
        public static readonly ImageBrush Enemy = new ImageBrush(new BitmapImage(new Uri("images/enemy.png", UriKind.Relative)));
        public static readonly ImageBrush Construction = new ImageBrush(new BitmapImage(new Uri("images/construction.png", UriKind.Relative)));
        public static readonly ImageBrush Bullet = new ImageBrush(new BitmapImage(new Uri("images/bullet.png", UriKind.Relative)));
        public static readonly ImageBrush Dead = new ImageBrush(new BitmapImage(new Uri("images/dead.png", UriKind.Relative)));
    }
}
