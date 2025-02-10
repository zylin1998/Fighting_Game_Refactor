using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class PropertyMonitor : MonoBehaviour
    {
        [SerializeField]
        private List<Property<int>>   _Integers = new();
        [SerializeField]
        private List<Property<float>> _Floats   = new();
        [SerializeField]
        private List<Property<bool>>  _Booleans = new();
        [SerializeField]
        private List<PropertyModel>   _Models   = new();

        public void Add(Property<int> property) 
        {
            _Integers.Add(property);
        }

        public void Add(Property<float> property)
        {
            _Floats.Add(property);
        }

        public void Add(Property<bool> property)
        {
            _Booleans.Add(property);
        }

        public void Add(PropertyModel model) 
        {
            _Models.Add(model);
        }

        public bool Remove(Property<int> property) 
        {
            return _Integers.Remove(property);
        }

        public bool Remove(Property<float> property)
        {
            return _Floats.Remove(property);
        }

        public bool Remove(Property<bool> property)
        {
            return _Booleans.Remove(property);
        }

        public bool Remove(PropertyModel model)
        {
            return _Models.Remove(model);
        }

        public Property<int> GetInteger(string id) 
        {
            return _Integers.Find(p => Equals(p.Id, id));
        }

        public Property<float> GetFloat(string id)
        {
            return _Floats.Find(p => Equals(p.Id, id));
        }

        public Property<bool> GetBoolean(string id)
        {
            return _Booleans.Find(p => Equals(p.Id, id));
        }

        public T GetModel<T>() where T : PropertyModel 
        {
            foreach (var model in _Models) 
            {
                if (model is T result) { return result; }
            }

            return default(T);
        }

        public T Get<T>() 
        {
            foreach (var property in _Integers) 
            {
                if (property is T result) { return result; }
            }

            foreach (var property in _Floats)
            {
                if (property is T result) { return result; }
            }

            foreach (var property in _Booleans)
            {
                if (property is T result) { return result; }
            }

            return default;
        }

        public IEnumerable<T> GetAll<T>()
        {
            foreach (var property in _Integers)
            {
                if (property is T result) { yield return result; }
            }

            foreach (var property in _Floats)
            {
                if (property is T result) { yield return result; }
            }

            foreach (var property in _Booleans)
            {
                if (property is T result) { yield return result; }
            }
        }

        public void ToStandard() 
        {
            _Integers.ForEach(p => p.Reset());
            _Floats  .ForEach(p => p.Reset());
            _Booleans.ForEach(p => p.Reset());
        }

        public void Clear() 
        {
            _Integers.Clear();
            _Floats  .Clear();
            _Booleans.Clear();
        }
    }
}