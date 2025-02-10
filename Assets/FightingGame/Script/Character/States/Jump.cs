using System;
using System.Linq;
using System.Collections.Generic;
using StateMachine;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Jump", menuName = "FightingGame/Character/State/Jump", order = 1)]
    public class Jump : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var jump     = character.GetModel<JumpModel>();
            var movement = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();
            var animator = character.GetModel<AnimatorModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .EnterWhen((c) => (jump.Jumped || !movement.IsGround) && !health.InActive)
                .ExitWhen((c) => movement.IsGround || health.InActive)
                .DoOnEnter((c) =>
                {
                    animator.Play("Jump");
                    
                    jump.Update();
                })
                .DoTick((c) => movement.Check())
                .DoFixedTick((c) => movement.Update())
                .WithId("Jump");
        }
    }
}
