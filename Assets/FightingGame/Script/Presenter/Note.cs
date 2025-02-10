using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using Loyufei.DomainEvents;

namespace FightingGame
{
    public struct Note : IDomainEvent
    {
        public Note(object id, object data)
        {
            EventId = id;
            Data = data;
        }

        public object EventId { get; }
        public object Data { get; }
    }
}
