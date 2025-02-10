using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Require Asset", menuName = "FightingGame/Character/Asset/RequireAssets", order = 1)]
    public class CharacterRequireAsset : RequireAssetBase
    {
        [SerializeField]
        private List<CharacterRequire> _Requires;

        public CharacterRequire this[string character] 
            => _Requires.FirstOrDefault(r => r.Character == character);

        public IEnumerable<CharacterRequire> Requires => _Requires;

        public override IEnumerable<string> All
        {
            get 
            {
                foreach (var require in _Requires) 
                {
                    foreach (var key in require.All) 
                    {
                        yield return key;
                    }
                }
            }
        }

        public IEnumerable<string> AllWithFacade
        {
            get
            {
                foreach (var require in _Requires)
                {
                    foreach (var key in require.AllWithFacade)
                    {
                        yield return key;
                    }
                }
            }
        }
    }
}
