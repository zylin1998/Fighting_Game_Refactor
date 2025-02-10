using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;

namespace FightingGame
{
    [Serializable]
    public class ConfigProperty
    {
        [SerializeField]
        private List<Property<int>>   _Integers = new();
        [SerializeField]
        private List<Property<float>> _Floats   = new();
        [SerializeField]
        private List<Property<bool>>  _Booleans = new();

        public Property<int> GetInteger(object id, int origin) 
        {
            var property = _Integers.Find(p => p.Id.Equals(id));

            if (property.IsDefault()) 
            {
                property = new Property<int>((string)id, origin);

                _Integers.Add(property);
            }

            return property;
        }

        public Property<float> GetFloat(object id, float origin)
        {
            var property = _Floats.Find(p => p.Id.Equals(id));
            
            if (property.IsDefault())
            {
                property = new Property<float>((string)id, origin);

                _Floats.Add(property);
            }
            
            return property;
        }

        public Property<bool> GetBoolean(object id, bool origin)
        {
            var property = _Booleans.Find(p => p.Id.Equals(id));

            if (property.IsDefault())
            {
                property = new Property<bool>((string)id, origin);

                _Booleans.Add(property);
            }

            return property;
        }
    }
}
