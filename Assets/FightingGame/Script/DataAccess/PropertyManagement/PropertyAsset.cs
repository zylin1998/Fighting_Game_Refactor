using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Stats", menuName = "FightingGame/Character/Stat/Asset", order = 1)]
    public class PropertyAsset : ScriptableObject
    {
        [SerializeField]
        private string _Character;

        [SerializeField]
        private List<Property<int>>   _Integers;
        [SerializeField]
        private List<Property<float>> _Floats;
        [SerializeField]
        private List<Property<bool>>  _Booleans;

        public string Character => _Character;

        public IEnumerable<Property<int>>   Integers => _Integers;
        public IEnumerable<Property<float>> Floats   => _Floats;
        public IEnumerable<Property<bool>>  Booleans => _Booleans;
    }
}
