using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "RequireItem", menuName = "FightingGame/Inventory/RequireItem", order = 1)]
    public class RequireItemAsset : ScriptableObject
    {
        [SerializeField]
        private List<string> _Items = new();

        public IEnumerable<string> Items => _Items.Select(i => i + "_Facade");
    }
}
