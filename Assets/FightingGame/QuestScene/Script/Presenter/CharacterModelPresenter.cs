using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace FightingGame.QuestScene
{
    internal class CharacterModelPresenter : EventNotesPresenter
    {
        public override object GroupId => Group.Gaming;

        [Inject]
        public CharacterModels CharacterModels { get; }

        protected override void Init()
        {
            Add(Notes.TaleDamage, TakeDamage);
        }

        private void TakeDamage(object data) 
        {
            var damage = (TakeDamage)data;

            CharacterModels.TakeDamage(damage);
        }
    }
}
