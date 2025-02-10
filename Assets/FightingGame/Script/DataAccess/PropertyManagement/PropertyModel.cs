using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    [Serializable]
    public class PropertyModel
    {
        [SerializeField]
        private string _Name;

        public PropertyModel() 
        {
            _Name = GetType().Name;
        }

        public virtual void Update() 
        {

        }
    }

    public class PropertyModel<T> : PropertyModel
    {
        public PropertyModel()
        {
            
        }

        public virtual T Update(T data)
        {
            return default(T);
        }
    }

    public class CharacterPropertyModel : PropertyModel
    {
        public CharacterPropertyModel(Character character)
        {
            Character = character;
        }

        public Character Character { get; }
    }

    public class CharacterPropertyModel<T> : PropertyModel<T>
    {
        public CharacterPropertyModel(Character character)
        {
            Character = character;
        }

        public Character Character { get; }
    }

}
