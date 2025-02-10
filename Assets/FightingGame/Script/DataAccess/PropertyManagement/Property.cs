using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [Serializable]
    public class Property<T>
    {
        public Property() : this(string.Empty, default)
        {
            
        }

        public Property(string id) : this(id, default)
        {

        }

        public Property(string id, T value) 
        {
            _Id = id;

            Set(value);
        }

        [SerializeField]
        protected string _Id;
        [SerializeField]
        protected T      _Value;

        public object Id       => _Id;
        public T      Value    => _Value;
        
        public virtual void Set(T value)
        {
            _Value = value;
        }

        public virtual void Reset() 
        {
            _Value = default;
        }
    }

    [Serializable]
    public class StandardProperty<T> : Property<T>
    {
        public StandardProperty(string id) : this(id, default) 
        {

        }

        public StandardProperty(string id, T standard) : base(id)
        {
            SetStandard(standard);
            Reset();
        }

        [SerializeField]
        protected T _Standard;

        public T Standard => _Standard;

        public void SetStandard(T standard)
        {
            _Standard = standard;
        }

        public override void Reset() 
        {
            _Value = Standard;
        }
    }
}