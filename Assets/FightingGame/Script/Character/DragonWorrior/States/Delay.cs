using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Delay", menuName = "FightingGame/Character/DragonWorrior/State/Delay", order = 1)]
    public class Delay : StateAssetBase<Character>
    {
        [SerializeField]
        private float _DelayMultipler = 1f;

        public override IState GetState(Character character)
        {
            var delay    = character.GetFloat("Delay");
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            var time = 0f;

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => time <= 0)
                .DoOnEnter((c) =>
                {
                    movement.Flip();

                    animator.Play("Idle");
                    
                    time = delay.Value * _DelayMultipler;
                })
                .DoFixedTick((c) => time -= Time.fixedDeltaTime)
                .WithId(_Id);
        }
    }
}
