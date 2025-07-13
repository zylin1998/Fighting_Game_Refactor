using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Hurt", menuName = "FightingGame/Character/State/Hurt", order = 1)]
    public class Hurt : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();

            return StateMachine.FunctionalState(character)
                .EnterWhen((c) => health.IsHurt)
                .ExitWhen((c) => animator.NormalizeTime("Hurt") >= 0.9f || health.IsDead)
                .DoOnEnter((c) =>
                {
                    health.Hurt.Set(false);

                    animator.Play("Hurt");
                })
                .WithId("Hurt");
        }
    }
}
