using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Attack", menuName = "FightingGame/Character/State/Attack", order = 1)]
    public class Attack : StateAssetBase<Character>
    {
        [SerializeField]
        private int _AttackCount;

        public override IState GetState(Character character)
        {
            var attackId   = _Id + _AttackCount;
            var attack     = character.GetBoolean(_Id);
            var damageArea = character.GetAsset<RangeDamageArea>(character.name + "_" + attackId);
            var animator   = character.GetModel<AnimatorModel>();
            var movement   = character.GetModel<MovementModel>();
            var health     = character.GetModel<HealthModel>();
            
            return StateMachine.StateMachine.FunctionalState(character)
                .EnterWhen((c) => movement.IsGround && attack.Value && !health.InActive)
                .ExitWhen ((c) => animator.NormalizeTime(attackId) >= 0.9f || health.InActive)
                .DoOnEnter((c) =>
                {
                    movement.Update(Vector2.zero);

                    animator.Play(attackId); 
                })
                .DoOnExit ((c) => damageArea.Set(false))
                .DoTick((c) => damageArea.Set(animator.NormalizeTime(attackId)))
                .WithId(attackId);
        }
    }
}
