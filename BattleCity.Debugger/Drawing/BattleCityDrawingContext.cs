using BattleCity.Client.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Media;

namespace BattleCity.Debugger.Drawing
{
    public class BattleCityDrawingContext
    {
        private readonly DrawingContext drawingContext;
        public Size RenderSize { get; set; }

        public GameState GameState { get; }

        private const int TileSize = 26;

        public BattleCityDrawingContext(GameState gameState, DrawingContext drawingContext)
        {
            this.drawingContext = drawingContext;
            GameState = gameState;
            RenderSize = GetRenderSize();
        }

        public void DrawTileText(string text, double size, Brush brush, Pen pen, int x, int y)
        {
            drawingContext.PushTransform(new TranslateTransform(
                x * TileSize + TileSize / 2,
                RenderSize.Height - TileSize - y * TileSize + TileSize / 2
                ));

            var formattedText = new FormattedText(
                text,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface("Verdana"),
                size,
                brush);

            drawingContext.DrawGeometry(
                brush, 
                pen, 
                formattedText.BuildGeometry(new System.Windows.Point(-formattedText.Width / 2, -formattedText.Height / 2)));

            drawingContext.Pop();
        }

        public void DrawTile(Brush brush, Pen pen, int x, int y, string direction)
        {
            var angle = GetDirectionAngle(direction);

            drawingContext.PushTransform(new TranslateTransform(
                x * TileSize + TileSize / 2,
                RenderSize.Height - TileSize - y * TileSize + TileSize / 2
                ));
            drawingContext.PushTransform(new RotateTransform(angle, 0, 0));
           
            drawingContext.DrawRectangle(brush, pen, 
                new Rect(-TileSize / 2, -TileSize / 2, TileSize, TileSize));

            drawingContext.Pop();
            drawingContext.Pop();
        }

        public void Clear()
        {
            drawingContext.DrawRectangle(Brushes.Black, null, new Rect(RenderSize));
        }

        private double GetDirectionAngle(string direction) => direction switch
        {
            "UP" => 180,
            "DOWN" => 0,
            "LEFT" => 90,
            "RIGHT" => 270,
            null => 0,
            _ => throw new NotImplementedException()
        };

        private Size GetRenderSize() => new Size(
            GameState.Borders.Max(border => (border.X + 1) * TileSize), 
            GameState.Borders.Max(border => (border.Y + 1) * TileSize));
    }
}
