using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class PlayerCharacterPool : CharacterPool
    {
        public PlayerCharacterPool([Inject(Id = "Player")] CharacterConstructor constructor) : base(constructor)
        {
            
        }

        public override object Group => "Player";
    }

    public class EnemyCharacterPool : CharacterPool
    {
        public EnemyCharacterPool([Inject(Id = "Enemy")] CharacterConstructor constructor) : base(constructor)
        {

        }

        public override object Group => "Enemy";
    }
}
