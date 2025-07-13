using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "FlyAttack", menuName = "FightingGame/Character/DragonWorrior/State/FlyAttack", order = 1)]
    public class FlyAttack : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            var anim = "FlyAttack";

            var shooter = character.GetAsset<Shooter>(character.name + "_Shooter1");
            var iblast = character.GetAsset<CoroutineDamageArea>(character.name + "_Iblast");

            shooter.Set(iblast);

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime(anim) >= 0.9)
                .DoOnEnter((c) =>
                {
                    shooter.Initialize();

                    movement.FreezeY = true;

                    movement.Flip();

                    animator.Play(anim);
                })
                .DoOnExit((c) => movement.FreezeY = false)
                .DoFixedTick((c) => shooter.Set(animator.NormalizeTime(anim) >= 0.66))
                .WithId(Id);
        }
    }
}
