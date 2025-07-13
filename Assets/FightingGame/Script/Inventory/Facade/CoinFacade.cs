using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachineX;

namespace FightingGame
{
    public class CoinFacade : ItemFacade
    {
        [SerializeField]
        private Rigidbody2D _Rigidbody;
        [SerializeField]
        private Animator    _Animator;
        [SerializeField, Min(0.1f)]
        private float       _DropDistance;
        
        public IStateMachine Machine { get; private set; }

        private IDisposable _Register;

        private float _Await = 0f;

        public override bool CanRelease => _Await <= 0f;

        private void Awake()
        {
            var drop = StateMachine.FunctionalState()
                .ExitWhen(() => _Rigidbody.velocity.y <= 0f)
                .DoOnEnter(() => transform.position += (Vector3.up * _DropDistance))
                .WithId(1);

            var crash = StateMachine.FunctionalState()
                .DoOnEnter(() => _Animator.Play("Open"))
                .WithId(2);

            var await = StateMachine.FunctionalState()
                .ExitWhen(() => _Await <= 0f)
                .DoOnEnter(() => _Await = 2f)
                .DoFixedTick(() => _Await -= Time.fixedDeltaTime)
                .WithId(3);

            Machine = StateMachine.SingleEntrance()
                .WithStates(drop, crash, await)
                .Sequence()
                .OrderBy(1, 2, 3);
        }

        public override void Enable()
        {
            _Register = Machine.FixedUpdate();
        }

        public override void Disable() 
        {
            _Register.Dispose();
        }
    }

    public class ItemFacade : MonoBehaviour 
    {
        public virtual bool CanRelease { get; }

        public virtual void Enable() 
        {

        }

        public virtual void Disable()
        {

        }
    }
}
