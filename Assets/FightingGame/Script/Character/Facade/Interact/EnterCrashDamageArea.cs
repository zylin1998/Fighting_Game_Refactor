using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class EnterCrashDamageArea : CoroutineDamageArea
    {
        [SerializeField]
        protected float _Speed;

        private void OnEnable()
        {
            Side = Character.transform.localScale.x > 0 ? 1f : -1f;
        }

        protected override void OnCollision(Collider2D collider)
        {
            base.OnCollision(collider);
            
            Crash(collider);
        }

        private void Crash(Collider2D collider)
        {
            Set(false);
        }

        public override bool Coroutine()
        {
            _Rigidbody.velocity = Direct * _Speed * Side;

            return gameObject.activeSelf;
        }
    }
}
