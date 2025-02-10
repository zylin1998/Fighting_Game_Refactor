using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class ObjectMonitor : MonoBehaviour
    {
        [SerializeField]
        private List<string> _Objects = new();

        public HashSet<object> Objects { get; } = new();

        private string Format = "{0} (Hash:{1})";

        public void Add(object obj) 
        {
            if (Objects.Add(obj)) 
            {
                _Objects.Add(string.Format(Format, obj.GetType().Name, obj.GetHashCode())); 
            }
        }

        public bool Remove(object obj)
        {
            var result = Objects.Remove(obj);

            if (result) 
            {
                _Objects.Remove(string.Format(Format, obj.GetType(), obj.GetHashCode())); 
            }

            return result;
        }

        public T Get<T>() 
        {
            return Objects.OfType<T>().FirstOrDefault();
        }
    }
}
