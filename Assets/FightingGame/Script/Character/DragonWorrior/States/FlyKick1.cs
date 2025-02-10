using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "FlyKick1", menuName = "FightingGame/Character/DragonWorrior/State/FlyKick1", order = 1)]
    public class FlyKick1 : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("FlyKick1") >= 0.9)
                .DoOnEnter((c) =>
                {
                    movement.Flip();

                    movement.FreezeY = true;

                    animator.Play("FlyKick1");
                })
                .DoFixedTick((c) => movement.Update(Vector2.zero))
                .WithId("FlyKick1");
        }
    }
}
