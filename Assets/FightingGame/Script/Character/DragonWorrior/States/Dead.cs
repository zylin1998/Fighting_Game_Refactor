using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Dead", menuName = "FightingGame/Character/DragonWorrior/State/Dead", order = 1)]
    public class Dead : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => false)
                .DoOnEnter((c) => animator.Play("Death"))
                .WithId(_Id);
        }
    }
}
