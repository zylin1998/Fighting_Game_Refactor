using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "RequireAsset", menuName = "FightingGame/Asset/RequireAsset", order = 1)]
    public class RequireAsset : RequireAssetBase
    {
        [SerializeField]
        private List<string> _Objects           = new();
        [SerializeField]
        private List<string> _ScriptableObjects = new();
        [SerializeField]
        private List<string> _GameObjects       = new();

        public override IEnumerable<string> All 
        {
            get 
            {
                foreach (var key in _Objects          ) { yield return key; }
                foreach (var key in _ScriptableObjects) { yield return key; }
                foreach (var key in _GameObjects      ) { yield return key; }
            }
        }
    }

    public class RequireAssetBase : ScriptableObject 
    {
        public virtual IEnumerable<string> All { get; } = new string[] { };
    }
}
