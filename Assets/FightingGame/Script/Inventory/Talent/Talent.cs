using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.ItemManagement;
using Zenject;

namespace FightingGame
{
    public class Talent : ItemManageModel
    {
        [Inject]
        protected override void Construct([Inject(Id = "Talent")] IItemStorage storage, [Inject(Id = "Talent")] IItemCollection collection, [Inject(Id = "Talent")] IItemLimitation limitation)
        {
            base.Construct(storage, collection, limitation);
        }
    }
}
