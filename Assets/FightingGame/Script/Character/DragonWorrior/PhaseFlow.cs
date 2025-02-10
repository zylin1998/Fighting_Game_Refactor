using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame.DragonWorrior
{
    [CreateAssetMenu(fileName = "PhaseFlow", menuName = "FightingGame/Character/DragonWorrior/StateMachine/PhaseFlow", order = 1)]
    public class PhaseFlow : PhaseStateMachineAsset<Character>
    {
        [SerializeField, Range(-1, 100)]
        private int _LeavePresentage;

        protected override IPhaseStateMachine<Character> CreateMachine(Character character)
        {
            var health     = character.GetModel<HealthModel>();
            var presentage = _LeavePresentage / 100f;

            return base.CreateMachine(character)
                .ExitWhen((c) => health.Normalized <= presentage);
        }
    }
}
