using Loyufei;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class SaveSystemModel : Loyufei.SaveSystemModel
    {
        [Inject]
        public SaveSystemModel(IEnumerable<ISaveSystem> saves) : base(saves)
        {

        }
    }
}
