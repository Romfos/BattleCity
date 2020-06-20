using BattleCity.Client.Models;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace BattleCity.Framework
{
    public class Navigation
    {
        private readonly GameState gameState;
        private readonly Predicates predicates;

        public Dictionary<Vector, ImmutableList<Vector>> Map { get; set; }
        public KeyValuePair<Vector, ImmutableList<Vector>>? Target { get; set; }

        public Navigation(GameState gameState, Predicates predicates)
        {
            this.gameState = gameState;
            this.predicates = predicates;
            Map = CreateStepMap(gameState.PlayerTank);
            Target = GetTarget();
        }

        public int? GetStepCountToTarget() => Target.HasValue ? Target.Value.Value.Count : (int?)null;

        public Vector GetTargetDirection() => Target.Value.Value.First() - gameState.PlayerTank;

        private KeyValuePair<Vector, ImmutableList<Vector>>? GetTarget() => Map
            .Where(x => predicates.IsEnemyTank(x.Key))
            .OrderBy(x => x.Value.Count)
            .Take(1)
            .FirstOrDefault();

        private Dictionary<Vector, ImmutableList<Vector>> CreateStepMap(Vector position)
        {
            var steps = new Dictionary<Vector, ImmutableList<Vector>>()
            {
                [position] = ImmutableList<Vector>.Empty
            };

            var next = new List<Vector> { position };
            while (next.Any())
            {
                next = next.SelectMany(x => Blow(x, steps)).ToList();
            }

            return steps;
        }

        private IEnumerable<Vector> Blow(Vector position, Dictionary<Vector, ImmutableList<Vector>> steps)
        {
            foreach (var nextPosition in Vector.Around(position).Where(predicates.IsVisible))
            {
                if (steps.ContainsKey(nextPosition))
                {
                    if (steps[nextPosition].Count > steps[position].Count + 1)
                    {
                        steps[nextPosition] = steps[position].Add(nextPosition);
                        yield return nextPosition;
                    }
                }
                else
                {
                    steps[nextPosition] = steps[position].Add(nextPosition);
                    yield return nextPosition;
                }
            }
        }
    }
}
