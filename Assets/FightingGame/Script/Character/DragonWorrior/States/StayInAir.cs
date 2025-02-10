using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;
using System.Linq;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "StayInAir", menuName = "FightingGame/Character/DragonWorrior/State/StayInAir", order = 1)]
    public class StayInAir : StateAssetBase<Character>
    {
        [SerializeField]
        private float _Period;
        [SerializeField]
        private int   _Loop;
        [SerializeField]
        private int   _Drop;

        public override IState GetState(Character character)
        {
            var movement = character.GetModel<MovementModel>();
            var animator = character.GetModel<AnimatorModel>();

            var coroutines = character.GetAssetAll<CoroutineDamageArea>();

            var length = 18;

            var groups = coroutines
                .GroupBy(c => c.name)
                .Where(g => g.Count() == length)
                .Select(g => g.ToArray())
                .ToList();

            var pairs = new List<IEnumerable<CoroutineDamageArea>>();

            for (var index = 0; index < length; index++) 
            {
                pairs.Add(new[] { groups[0][index], groups[1][index] });
            }

            var shooter = character.GetAsset<PluralShooter>(character.name + "_Shooter3");

            shooter.Set(pairs);

            var time = 0f;
            var loop = 0;

            return StateMachine.StateMachine.FunctionalState(character)
                .ExitWhen((c) => loop >= _Drop)
                .DoOnEnter((c) =>
                {
                    time = 0f;
                    loop = 0;

                    movement.FreezeY = true;

                    animator.Play("FlyAttack");
                })
                .DoOnExit((c) => movement.FreezeY = false)
                .DoFixedTick((c) =>
                {
                    if (time <= 0) 
                    {
                        if (loop <= _Loop) 
                        {
                            shooter.Set(loop);
                        }
                        
                        loop++;
                        
                        time = _Period;
                    }

                    time -= Time.fixedDeltaTime;
                })
                .WithId(_Id);
        }
    }
}
