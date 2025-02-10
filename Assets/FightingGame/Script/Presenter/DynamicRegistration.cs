using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using Loyufei.MVP;
using Loyufei.DomainEvents;

namespace FightingGame
{
    public class DynamicRegistration : Presenter
    {
        public override object GroupId => "FightingGame";

        public void Register(DynamicPoster poster) 
        {
            poster.Subscribe((e) => SettleEvents(poster.GroupId, e));
        }
    }
}
