using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    public class TrackingModel : CharacterPropertyModel
    {
        public TrackingModel(Character character) : base(character)
        {

        }

        public Character Target { get; private set; }

        public HealthModel TargetHealth { get; private set; }

        public Vector3 Distance => Target.Position - Character.Position;

        public float Magnitude => Distance.magnitude;

        public bool Tracking => !TargetHealth.IsDead;

        public void SetTarget(Character target) 
        {
            Target = target;

            TargetHealth = target.GetModel<HealthModel>();
        }
    }
}
