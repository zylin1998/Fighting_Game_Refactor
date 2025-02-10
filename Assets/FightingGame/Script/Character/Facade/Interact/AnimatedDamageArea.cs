using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace FightingGame
{
    public class AnimatedDamageArea : CoroutineDamageArea
    {
        [SerializeField]
        private Animator   _Animator;
        
        public override void Set(bool active)
        {
            base.Set(active);

            if (active) { _Animator.Play("Idle"); }
        }

        public override bool Coroutine()
        {
            var state = _Animator.GetCurrentAnimatorStateInfo(0);

            _Collider.enabled = state.IsName("Idle");

            if (state.IsName("End") && state.normalizedTime >= 0.9f) { Set(false); }

            return base.Coroutine();
        }
    }
}
