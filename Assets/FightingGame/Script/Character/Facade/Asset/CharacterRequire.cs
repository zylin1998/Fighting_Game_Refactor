using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [Serializable]
    public class CharacterRequire
    {
        [SerializeField]
        private string _Character;

        [SerializeField]
        private string _Facade;

        [SerializeField]
        private List<string> _GameObjects;
        [SerializeField]
        private List<string> _ScriptableObjects;
        [SerializeField]
        private List<string> _Objects;

        private string Format = "{0}_{1}";

        public string Character => _Character;
        public string Facade    => string.Format(Format, _Character, _Facade);

        public IEnumerable<string> GameObjects       => _GameObjects      .Select(s => string.Format(Format, _Character, s));
        public IEnumerable<string> ScriptableObjects => _ScriptableObjects.Select(s => string.Format(Format, _Character, s));
        public IEnumerable<string> Objects           => _Objects          .Select(s => string.Format(Format, _Character, s));

        public IEnumerable<string> All           => Objects.Concat(ScriptableObjects).Concat(GameObjects);
        public IEnumerable<string> AllWithFacade => new[] { Facade }.Concat(All);
    }
}
