using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Strike1", menuName = "FightingGame/Character/DragonWorrior/State/Strike1", order = 1)]
    public class Strike1 : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("Strike1") >= 0.9)
                .DoOnEnter((c) =>
                {
                    movement.Flip();

                    animator.Play("Strike1");
                })
                .WithId("Strike1");
        }
    }
}
