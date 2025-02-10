using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "QuestInfo", menuName = "FightingGame/Quest/Info", order = 1)]
    public class QuestInfo : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int          _Id;
        [SerializeField]
        private int          _EnvironmentId;
        [SerializeField]
        private string       _BGM;
        [SerializeField]
        private float        _GameTime;
        [SerializeField]
        private string       _Player;
        [SerializeField]
        private List<string> _Enemies;
        [SerializeField]
        private float        _SpawnTime;
        [SerializeField]
        private bool         _UseSeeds;
        [SerializeField]
        private List<int>    _Seeds;

        public int                 Id          => _Id;
        public string              Environment => "Environment" + _EnvironmentId;
        public string              BGM         => _BGM;
        public float               GameTime    => _GameTime;
        public string              Player      => _Player;
        public IEnumerable<string> Enemies     => _Enemies;
        public float               SpawnTime   => _SpawnTime;
        public bool                UseSeeds    => _UseSeeds;
        public IEnumerable<int>    Seeds       => _Seeds.Any() ? _Seeds : DefaultSeeds;

        public IEnumerable<string> Characters 
            => new[] { Player }.Concat(Enemies).Distinct();

        private static IEnumerable<int> DefaultSeeds { get; } 
            = Enumerable.Range(1, 1000).ToArray();
    }
}
