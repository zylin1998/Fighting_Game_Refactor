using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using Loyufei.ItemManagement;

namespace FightingGame
{
    public class TalentMonitor : ItemManageMonitor
    {
        [Inject]
        protected override void Construct([Inject(Id = "Talent")] IItemCollection collection, [Inject(Id = "Talent")] IItemStorage storage)
        {
            base.Construct(collection, storage);
        }
    }
}
