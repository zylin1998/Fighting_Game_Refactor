using StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Tracking", menuName = "FightingGame/Character/State/Tracking", order = 1)]
    public class Tracking : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var movement = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();
            var animator = character.GetModel<AnimatorModel>();

            return StateMachine.StateMachine.FunctionalState(character)
                .EnterWhen((c) => movement.Moving && !health.InActive)
                .ExitWhen ((c) => !movement.Moving || health.InActive)
                .DoOnEnter((c) => animator.Play("Move"))
                .DoTick((c) => movement.Check())
                .DoFixedTick((c) => movement.Update())
                .WithId(_Id);
        }
    }
}
