using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class CharacterConstructor
    {
        public virtual void ConstructProperty(Character character)
        {
            character.Install(new StandardProperty<bool>("Attack"));
            character.Install(new StandardProperty<bool>("Jump"));
            character.Install(new StandardProperty<bool>("Hurt"));
            character.Install(new StandardProperty<bool>("GroundCheck"));
            character.Install(new StandardProperty<bool>("Dead"));

            character.Install(new StandardProperty<float>("JumpForce"));
            character.Install(new StandardProperty<float>("Damage"));
            character.Install(new StandardProperty<float>("Health"));
            character.Install(new StandardProperty<float>("Defend"));
            character.Install(new StandardProperty<float>("MoveX"));
            character.Install(new StandardProperty<float>("MoveY"));
            character.Install(new StandardProperty<float>("MoveSpeed"));
        }

        public virtual void ConstructModel(Character character)
        {
            character.Install(new MovementModel(character));
            character.Install(new JumpModel(character));
            character.Install(new AnimatorModel(character));
            character.Install(new DamageCalculateModel(character));
        }

        public virtual void ConstructStats(Character character)
        {
            var stats = character.GetAsset<PropertyAsset>();

            foreach (var stat in stats.Integers)
            {
                var property = (StandardProperty<int>)character.GetInteger(stat.Id.ToString());

                property.SetStandard(stat.Value);
                property.Reset();
            }

            foreach (var stat in stats.Floats)
            {
                var property = (StandardProperty<float>)character.GetFloat(stat.Id.ToString());

                property.SetStandard(stat.Value);
                property.Reset();
            }

            foreach (var stat in stats.Booleans)
            {
                var property = (StandardProperty<bool>)character.GetBoolean(stat.Id.ToString());

                property.SetStandard(stat.Value);
                property.Reset();
            }
        }

        public void AssetInstalled(Character character, IEnumerable<Object> objects)
        {
            foreach (var obj in objects)
            {
                character.Install(obj);
            }
        }

        public void Reset(Character character) 
        {
            character.Properties.ToStandard();
        }
    }
}
