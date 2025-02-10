using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Delay", menuName = "FightingGame/Character/State/Delay", order = 1)]
    public class Delay : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var delay    = character.GetFloat("Delay");
            var attack   = character.GetBoolean("Attack");
            var animator = character.GetModel<AnimatorModel>();
            var move     = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();

            var time = delay.Value;

            return StateMachine.StateMachine.FunctionalState(character)
                .EnterWhen((c) => !move.Moving && attack.Value && !health.InActive)
                .ExitWhen ((c) => time <= 0f || move.Moving || health.InActive)
                .DoOnEnter((c) =>
                { 
                    time = delay.Value;

                    move.Update(Vector2.zero);

                    animator.Play("Idle");
                })
                .DoFixedTick((c) => time -= Time.fixedDeltaTime)
                .DoTick((c) => move.Check())
                .WithId(_Id);
        }
    }
}
