using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "Jump", menuName = "FightingGame/Character/DragonWorrior/State/Jump", order = 1)]
    public class Jump : StateAssetBase<Character>
    {
        [SerializeField, Range(-1, 1)]
        private int _Side;
        [SerializeField]
        private float _Distance;

        public override IState GetState(Character character) 
        {
            var tracking = character.GetModel<TrackingModel>();
            var movement = character.GetModel<MovementModel>();
            var animator = character.GetModel<AnimatorModel>();
            var jump     = character.GetModel<JumpModel>();

            var damageArea = character.GetAsset<DamageArea>(character.name + "_Touch1");

            var direct = Vector2.zero;
            var speed  = 0f;

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => movement.IsGround)
                .DoOnEnter((c) =>
                {
                    animator.Play("Jump");
                    
                    jump.ForceUpdate();
                    
                    movement.Flip();

                    direct = new Vector2(_Side == 0 ? tracking.Distance.x : _Side, 0);
                    speed = (tracking.Distance.x + _Side * _Distance).Abs().Clamp(0, 15);
                })
                .DoOnExit((c) =>
                {
                    movement.Move(0, Vector2.zero);

                    damageArea.Set(false);
                })
                .DoFixedTick((c) =>
                {
                    movement.Move(speed, direct);

                    damageArea.Set(c.Rigidbody.velocity.y <= 0);
                })
                .DoTick((c) => movement.Check())
                .WithId(_Id);
        }
    }
}
