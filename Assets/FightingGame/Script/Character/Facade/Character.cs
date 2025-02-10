using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using StateMachine;

namespace FightingGame
{
    public partial class Character : MonoDataAccess
    {
        [SerializeField]
        private Rigidbody2D _Rigidbody;
        [SerializeField]
        private Animator    _Animator;
        [SerializeField]
        private Transform   _Model;
        [SerializeField]
        private Transform   _CamerFocus;
        [SerializeField]
        private LayerMask   _GroundMask;

        private IDisposable _Update;
        private IDisposable _FixedUpdate;

        public IStateMachine Machine { get; protected set; }

        public Animator    Animator    => _Animator;
        public Rigidbody2D Rigidbody   => _Rigidbody;
        public Transform   Model       => _Model;
        public Transform   CameraFocus => _CamerFocus;
        public LayerMask   GroundMask  => _GroundMask;
        public Vector3     Position    => transform.position;

        public string Id   { get; set; }
        public int    GUID { get; set; }

        public void CreateStateMachine() 
        {
            Machine = this.GetAsset<IStateMachineAsset<Character>>().GetMachine(this);
        }

        public void Enable() 
        {
            _Update      = Machine.Update();
            _FixedUpdate = Machine.FixedUpdate(false);
        }

        public void Disable()
        {
            _Update     ?.Dispose();
            _FixedUpdate?.Dispose();
        }
    }
}