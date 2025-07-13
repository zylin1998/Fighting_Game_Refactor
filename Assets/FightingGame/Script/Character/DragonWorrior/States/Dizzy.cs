using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "Dizzy", menuName = "FightingGame/Character/DragonWorrior/State/Dizzy", order = 1)]
    public class Dizzy : StateAssetBase<Character>
    {
        [SerializeField]
        private float _DelayMultipler = 1f;

        public override IState GetState(Character character)
        {
            var delay    = character.GetFloat("Delay");
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();
            var health   = character.GetModel<HealthModel>();

            var time = 0f;

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => time <= 0 || health.InActive)
                .DoOnEnter((c) =>
                {
                    health.Hurt.Reset();

                    movement.Flip();

                    animator.Play("Dizzy");

                    time = delay.Value * _DelayMultipler;
                })
                .DoOnExit((c) => health.Hurt.Reset())
                .DoFixedTick((c) => time -= Time.fixedDeltaTime)
                .WithId(_Id);
        }
    }
}
