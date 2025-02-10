using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;
using UnityEngine;

namespace FightingGame
{
    public class PlayerCharacterConstructor : CharacterConstructor
    {
        [Inject]
        public TalentMonitor Talent { get; }
        [Inject(Id = "Extra")]
        public PropertyAsset Extra  { get; }

        public override void ConstructModel(Character character)
        {
            base.ConstructModel(character);

            character.Install(new HealthModel(character, true));
        }

        public override void ConstructStats(Character character)
        {
            base.ConstructStats(character);

            var talents = Talent
                .GetAll()
                .Select(stack => (stack.Item.Name, stack.Count));
            
            foreach (var talent in talents) 
            {
                var extra = Extra.Floats.FirstOrDefault((s) => s.Id.Equals(talent.Name));

                if (extra == null) { continue; }

                var stat = (StandardProperty<float>)character.GetFloat(talent.Name);

                stat.SetStandard(stat.Standard + extra.Value * talent.Count);
            }
        }
    }

    public class EnemyCharacterConstructor  : CharacterConstructor
    {
        public override void ConstructProperty(Character character)
        {
            base.ConstructProperty(character);

            character.Install(new StandardProperty<int>("Gather"));

            character.Install(new StandardProperty<float>("Delay"));
        }

        public override void ConstructModel(Character character)
        {
            base.ConstructModel(character);

            character.Install(new HealthModel(character));
            character.Install(new TrackingModel(character));
        }
    }
}
