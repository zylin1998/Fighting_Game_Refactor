using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Loyufei.DomainEvents;
using Zenject;

namespace FightingGame 
{
    public class DynamicPoster : MonoBehaviour, IObservable<IDomainEvent>
    {
        public virtual object GroupId { get; set; }

        protected Subject<IDomainEvent> Subject { get; } = new();

        public virtual object EventId { get; set; }

        [Inject]
        protected void Construct(DynamicRegistration registration) 
        {
            registration.Register(this);
        }

        public IDisposable Subscribe(IObserver<IDomainEvent> observer) 
        {
            return Subject.Subscribe(observer);
        }
    }
}
