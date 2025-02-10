using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "CrouchAttack", menuName = "FightingGame/Character/DragonWorrior/State/CrouchAttack", order = 1)]
    public class CrouchAttack : StateAssetBase<Character>
    {
        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();

            var shooter = character.GetAsset<Shooter>(character.name + "_Shooter2");

            var fireball  = character.GetAsset<CoroutineDamageArea>(character.name + "_Fireball");
            var explosion = character.GetAsset<CoroutineDamageArea>(character.name + "_Explosion");

            shooter.Set(fireball, explosion);

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => animator.NormalizeTime("CrouchAttack") >= 0.9)
                .DoOnEnter((c) =>
                {
                    shooter.Initialize();

                    movement.Flip();

                    animator.Play("CrouchAttack");
                })
                .DoFixedTick((c) => shooter.Set(animator.NormalizeTime("CrouchAttack") >= 0.66))
                .WithId(Id);
        }
    }
}
