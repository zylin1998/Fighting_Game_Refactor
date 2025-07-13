using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "FlyKick2", menuName = "FightingGame/Character/DragonWorrior/State/FlyKick2", order = 1)]
    public class FlyKick2 : StateAssetBase<Character>
    {
        [SerializeField]
        private float _Time = 1.0f;

        public override IState GetState(Character character)
        {
            var animator = character.GetModel<AnimatorModel>();
            var movement = character.GetModel<MovementModel>();
            var tracking = character.GetModel<TrackingModel>();

            var damageArea = character.GetAsset<DamageArea>(character.name + "_Touch2");

            var time = 0f;
            var side = 0f;

            return StateMachine.FunctionalState(character)
                .ExitWhen((c) => time <= 0f)
                .DoOnEnter((c) =>
                {
                    time = _Time;

                    movement.Flip();

                    animator.Play("FlyKick2");

                    side = tracking.Distance.x;

                    damageArea.Set(true);
                })
                .DoOnExit((c) =>
                {
                    movement.Update(Vector2.zero);

                    damageArea.Set(false);
                })
                .DoFixedTick((c) => 
                {
                    time -= Time.fixedDeltaTime;

                    movement.Move(new Vector2(side, 0f));
                })
                .WithId("FlyKick2");
        }
    }
}
