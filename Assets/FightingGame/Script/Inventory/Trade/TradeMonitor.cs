using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei.ItemManagement;
using Zenject;

namespace FightingGame
{
    public class TradeMonitor : ItemTradeMonitor
    {
        [Inject]
        protected override void Construct([Inject(Id = "Trade")] IItemTrade trade, [Inject(Id = "Trade")] ITradeLog log)
        {
            base.Construct(trade, log);
        }
    }
}
