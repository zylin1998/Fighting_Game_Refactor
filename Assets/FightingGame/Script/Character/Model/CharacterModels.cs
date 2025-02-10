using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class CharacterModels
    {
        private GameReportModel _Report;

        [Inject]
        public CharacterCollection Collection { get; }
        [Inject]
        public GlobalDataAccess    DataAccess { get; }

        public GameReportModel Report 
            => _Report ?? (_Report = DataAccess.GetModel<GameReportModel>());

        public float TakeDamage(TakeDamage takeDamage) 
        {
            var group  = takeDamage.Tag;
            var guid   = takeDamage.GUID;
            var damage = takeDamage.Damage;
            var force  = takeDamage.Force;

            var character = Collection.Get(group, guid);
            var result    = character?.GetModel<HealthModel>()?.Update(damage);

            if (group == "Player") 
            {
                Report?.Injure(result ?? 0f);
            }
            
            character?.Rigidbody.AddForce(force * 1.7f, ForceMode2D.Impulse);

            return result ?? float.MaxValue;
        }
    }
}
