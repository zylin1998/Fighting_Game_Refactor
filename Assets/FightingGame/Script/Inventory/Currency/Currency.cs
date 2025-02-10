using System.Collections;
using System.Collections.Generic;
using Zenject;
using Loyufei.ItemManagement;
using UnityEngine;

namespace FightingGame
{
    public class Currency : ItemManageModel
    {
        [Inject]
        protected override void Construct([Inject(Id = "Currency")] IItemStorage storage, [Inject(Id = "Currency")] IItemCollection collection, [Inject(Id = "Currency")] IItemLimitation limitation)
        {
            base.Construct(storage, collection, limitation);
        }
    }
}
