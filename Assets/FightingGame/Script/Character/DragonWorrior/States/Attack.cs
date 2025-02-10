using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Attack", menuName = "FightingGame/Character/DragonWorrior/State/Attack", order = 1)]
    public class Attack : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            var shooter = character.GetAsset<Shooter>(character.name + "_Shooter1");
            var iblast = character.GetAsset<CoroutineDamageArea>(character.name + "_Iblast");

            shooter.Set(iblast);

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("Attack") >= 0.9)
                .DoOnEnter((c) =>
                {
                    shooter.Initialize();

                    movement.Flip();

                    animator.Play("Attack");
                })
                .DoFixedTick((c) => shooter.Set(animator.NormalizeTime("Attack") >= 0.66))
                .WithId(Id);
        }
    }
}
