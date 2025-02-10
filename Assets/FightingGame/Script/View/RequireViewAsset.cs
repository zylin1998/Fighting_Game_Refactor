using ExcelDataReader;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "RequireView", menuName = "FightingGame/View/RequireAsset", order = 1)]
    public class RequireViewAsset : RequireAssetBase
    {
        [SerializeField]
        private List<string> _Menus             = new();
        [SerializeField]
        private List<string> _Listeners         = new();
        [SerializeField]
        private List<string> _ScriptableObjects = new();
        [SerializeField]
        private List<string> _Objects           = new();
        [SerializeField]
        private string _Select;
        [SerializeField]
        private string _Click;

        public string Select => _Select;
        public string Click  => _Click;

        public IEnumerable<string> Menus     => _Menus    .Select(m => m + "_Menu");
        public IEnumerable<string> Listeners => _Listeners.Select(m => m + "_Listener");

        public override IEnumerable<string> All 
            => Menus.Concat(Listeners).Concat(_ScriptableObjects).Concat(_Objects);
    }
}
