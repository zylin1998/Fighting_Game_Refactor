using System;
using System.Linq;
using System.Collections.Generic;

namespace FightingGame
{
    public class EventNotes
    {
        private Dictionary<object, Group> Groups { get; } = new();

        public void Add(object groupId, object id, Action<object> action) 
        {
            GetGroup(groupId).Add(id, action);
        }

        public bool Remove(object groupId, object id) 
        {
            return GetGroup(groupId).Remove(id);
        }

        public bool Remove(object groupId, Action<object> callback)
        {
            return GetGroup(groupId).Remove(callback);
        }

        public void Invoke(object groupId, object id, object data) 
        {
            GetGroup(groupId).Invoke(id, data);
        }

        private Group GetGroup(object groupId) 
        {
            if (Groups.TryGetValue(groupId, out var group)) { return group; }

            group = new Group();

            Groups.Add(groupId, group);

            return group;
        }

        private class Group
        {
            public Dictionary<object, List<Action<object>>> Registration { get; } = new();

            public void Add(object id, Action<object> action)
            {
                if (Registration.TryGetValue(id, out var actions)) { actions.Add(action); }

                else { Registration.Add(id, new() { action }); }
            }

            public bool Remove(object id)
            {
                return Registration.Remove(id);
            }

            public bool Remove(Action<object> callback)
            {
                var pair = Registration.FirstOrDefault(p => p.Value.Contains(callback));

                return pair.Value.Remove(callback);
            }

            public void Invoke(object id, object data)
            {
                if (Registration.TryGetValue(id, out var actions))
                {
                    actions.ForEach(action => action.Invoke(data));
                }
            }
        }
    }
}
