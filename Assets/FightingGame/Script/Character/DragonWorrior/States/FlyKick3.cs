using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "FlyKick3", menuName = "FightingGame/Character/DragonWorrior/State/FlyKick3", order = 1)]
    public class FlyKick3 : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("FlyKick3") >= 0.9)
                .DoOnEnter((c) =>
                {
                    movement.FreezeY = false;

                    animator.Play("FlyKick3");
                })
                .DoFixedTick((c) => movement.Update(Vector2.zero))
                .WithId("FlyKick3");
        }
    }
}
