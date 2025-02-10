using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class RangeDamageArea : DamageArea
    {
        [SerializeField, Range(0f, 1f)]
        private float _EnableTime;
        [SerializeField, Range(0f, 1f)]
        private float _DisableTime;
        
        public void Set(float normalizeTime) 
        {
            Set(normalizeTime >= _EnableTime && normalizeTime <= _DisableTime);
        }
    }
}
