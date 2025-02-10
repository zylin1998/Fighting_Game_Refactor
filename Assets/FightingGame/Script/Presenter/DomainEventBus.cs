using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class DomainEventBus : Loyufei.DomainEvents.DomainEventBus
    {
        [Inject]
        public DomainEventBus() : base()
        {

        }
    }
}
