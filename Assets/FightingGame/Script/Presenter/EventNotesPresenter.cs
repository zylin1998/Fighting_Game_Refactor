using System;
using System.Linq;
using System.Collections.Generic;
using Loyufei.MVP;
using Loyufei.DomainEvents;

namespace FightingGame
{
    public class EventNotesPresenter : ModelPresenter<EventNotes>
    {
        protected override void RegisterEvents()
        {
            Register<Note>(Invoke);
        }

        protected void Invoke(Note noteEvent) 
        {
            Model.Invoke(GroupId, noteEvent.EventId, noteEvent.Data);
        }

        protected void Add(object id, Action<object> callback) 
        {
            Model.Add(GroupId, id, callback);
        } 
    }
}
