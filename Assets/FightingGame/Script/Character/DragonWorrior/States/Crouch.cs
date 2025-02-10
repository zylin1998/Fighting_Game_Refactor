using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Crouch", menuName = "FightingGame/Character/DragonWorrior/State/Crouch", order = 1)]
    public class Crouch : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("Crouch") >= 0.9)
                .DoOnEnter((c) =>
                {
                    movement.Flip();

                    animator.Play("Crouch");
                })
                .WithId(Id);
        }
    }
}
