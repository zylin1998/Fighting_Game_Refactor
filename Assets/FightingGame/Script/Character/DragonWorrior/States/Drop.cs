using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Drop", menuName = "FightingGame/Character/DragonWorrior/State/Drop", order = 1)]
    public class Drop : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();
            
            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => movement.IsGround)
                .DoOnEnter((c) =>
                {
                    animator.Play("Jump");
                })
                .DoTick((c) => movement.Check())
                .WithId(_Id);
        }
    }
}
