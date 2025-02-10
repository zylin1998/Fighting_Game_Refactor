using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "JumpTo", menuName = "FightingGame/Character/DragonWorrior/State/JumpTo", order = 1)]
    public class JumpTo : StateAssetBase<Character>
    {
        [SerializeField]
        private float _TargetHeight;
        public override IState GetState(Character character)
        {
            var movement = character.GetModel<MovementModel>();
            var animator = character.GetModel<AnimatorModel>();
            var jump     = character.GetModel<JumpModel>();

            var height = 0f;

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => (character.Position.y - height) >= _TargetHeight)
                .DoOnEnter((c) =>
                {
                    height = character.Position.y;

                    animator.Play("Jump");

                    jump.ForceUpdate();

                    movement.Flip();
                })
                .DoOnExit((c) => movement.Stop())
                .DoTick((c) => movement.Check())
                .WithId(_Id);
        }
    }
}
