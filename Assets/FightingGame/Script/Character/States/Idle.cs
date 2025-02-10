using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Idle", menuName = "FightingGame/Character/State/Idle", order = 1)]
    public class Idle : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var move     = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();
            var animator = character.GetModel<AnimatorModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .EnterWhen  ((c) => move.IsGround && !move.Moving && !health.InActive)
                .DoOnEnter  ((c) =>
                {
                    animator.Play("Idle");
                    
                    move.Update();
                })
                .DoTick((c) => move.Check())
                .WithId("Idle");
        }
    }
}