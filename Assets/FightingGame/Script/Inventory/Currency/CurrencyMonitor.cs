using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using Loyufei.ItemManagement;

namespace FightingGame
{
    public class CurrencyMonitor : ItemManageMonitor
    {
        [Inject]
        protected override void Construct([Inject(Id = "Currency")] IItemCollection collection, [Inject(Id = "Currency")] IItemStorage storage)
        {
            base.Construct(collection, storage);
        }
    }
}
