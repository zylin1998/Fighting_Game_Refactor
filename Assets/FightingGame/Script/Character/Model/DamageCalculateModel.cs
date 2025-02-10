using System;
using System.Linq;
using System.Collections.Generic;

namespace FightingGame
{
    public class DamageCalculateModel : CharacterPropertyModel
    {
        public DamageCalculateModel(Character character) : base(character)
        {
            DamageProperty = Character.GetFloat("Damage");
        }

        public Property<float> DamageProperty { get; }

        public float Damage => DamageProperty.Value;

        public float CalculatedDamage => DamageProperty.Value;

        public override void Update()
        {
            //no use
        }
    }
}
