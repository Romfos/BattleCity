using BattleCity.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BattleCity.Framework
{
    public struct Vector
    {
        public static Vector Up { get; } = new Vector(0, 1);
        public static Vector Down { get; } = new Vector(0, -1);
        public static Vector Right { get; } = new Vector(1, 0);
        public static Vector Left { get; } = new Vector(-1, 0);

        public static Vector[] Directions { get; } = { Up, Down, Left, Right };

        public static Vector FromDirection(string direction) => direction switch
        {
            "UP" => Up,
            "DOWN" => Down,
            "LEFT" => Left,
            "RIGHT" => Right,
            _ => throw new NotImplementedException()
        };

        public static IEnumerable<Vector> Ray(Vector position, Vector direction)
        {
            while (true)
            {
                yield return position;
                position += direction;
            }
        }

        public static IEnumerable<Vector> Around(Vector position) => Directions.Select(x => x + position);

        public int X { get; set; }
        public int Y { get; set; }

        public Vector(int x, int y)
        {
            X = x;
            Y = y;
        }

        public Vector(Point point)
        {
            X = point.X;
            Y = point.Y;
        }

        public static implicit operator Vector(Point point) => new Vector(point);
        public static bool operator ==(Vector left, Vector right) => left.Equals(right);
        public static bool operator !=(Vector left, Vector right) => !left.Equals(right);
        public static Vector operator +(Vector left, Vector right) => new Vector(left.X + right.X, left.Y + right.Y);
        public static Vector operator -(Vector left, Vector right) => new Vector(left.X - right.X, left.Y - right.Y);
        public static Vector operator *(Vector left, int scale) => new Vector(left.X * scale, left.Y * scale);

        public override bool Equals(object obj) => obj is Vector vector &&
                   X == vector.X &&
                   Y == vector.Y;

        public override int GetHashCode() => HashCode.Combine(X, Y);

        public Commands ToCommand()
        {
            if (this == Up)
            {
                return Commands.GO_TOP;
            }
            if (this == Down)
            {
                return Commands.GO_DOWN;
            }
            if (this == Left)
            {
                return Commands.GO_LEFT;
            }
            if (this == Right)
            {
                return Commands.GO_RIGHT;
            }
            throw new NotImplementedException();
        }
    }
}
