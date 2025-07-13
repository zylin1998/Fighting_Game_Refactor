using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Strike3", menuName = "FightingGame/Character/DragonWorrior/State/Strike3", order = 1)]
    public class Strike3 : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("Strike3") >= 0.9)
                .DoOnEnter((c) =>
                {
                    movement.Flip();

                    animator.Play("Strike3");
                })
                .WithId("Strike3");
        }
    }
}
