using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using Loyufei.ItemManagement;
using UnityEngine;

namespace FightingGame
{
    public class Trade : ItemTradeModel
    {
        [Inject]
        protected override void Construct([Inject(Id = "Trade")] IItemTrade trade, [Inject(Id = "Trade")] ITradeLog log)
        {
            base.Construct(trade, log);
        }
    }
}
