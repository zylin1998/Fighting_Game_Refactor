using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.ItemManagement;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "GameFile", menuName = "FightingGame/System/GameFile", order = 1)]
    public class IndependentFile : IndependentFileAsset<PlayerFile>
    {
        public override void InstallBindings()
        {
            base.InstallBindings();
        }
    }
}
