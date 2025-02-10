using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace FightingGame
{
    public class DataRefresher 
    {
        public Dictionary<object, List<Action<object>>> Texts { get; } = new();

        public void Register(object id, Action<object> callback) 
        {
            if (Texts.TryGetValue(id, out var list)) { list.Add(callback); }

            else { Texts.Add(id, new() { callback }); }
        }

        public bool Unregister(object id) 
        {
            return Texts.Remove(id);
        }

        public bool Unregister(Action<object> callback) 
        {
            var list = Texts.FirstOrDefault((p) => p.Value.Contains(callback)).Value;

            if (list != default) 
            {
                return list.Remove(callback);
            }

            return false;
        }

        public void Refresh(object id, object data) 
        {
            if (Texts.TryGetValue(id, out var list)) 
            {
                list.ForEach(text => text.Invoke(data));
            }
        }
    }
}
