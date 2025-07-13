using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Dead", menuName = "FightingGame/Character/State/Dead", order = 1)]
    public class Dead : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var health   = character.GetModel<HealthModel>();
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            return StateMachine.FunctionalState(character)
                .EnterWhen((c) => health.IsDead)
                .ExitWhen ((c) => false)
                .DoOnEnter((c) =>
                {
                    animator.Play("Dead");
                    movement.Update(Vector2.zero);
                })
                .WithId("Dead");
        }
    }
}
