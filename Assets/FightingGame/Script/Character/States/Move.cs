using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame 
{
    [CreateAssetMenu(fileName = "Move", menuName = "FightingGame/Character/State/Move", order = 1)]
    public class Move : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var movement = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();
            var animator = character.GetModel<AnimatorModel>();

            return StateMachine.FunctionalState(character)
                .EnterWhen  ((c) => movement.IsGround && movement.Moving && !health.InActive)
                .DoOnEnter  ((c) => c.Animator.Play("Move"))
                .DoTick     ((c) => movement.Check())
                .DoFixedTick((c) => movement.Update())
                .WithId("Move");
        }
    }
}
