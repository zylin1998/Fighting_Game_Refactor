using System;
using System.Linq;
using System.Collections.Generic;
using UniRx;
using Loyufei;
using UnityEngine;

namespace FightingGame
{
    public class HealthModel : CharacterPropertyModel<float>, IObservable<HealthModel.Result>
    {
        public HealthModel(Character character) : this(character, false) 
        {

        }

        public HealthModel(Character character, bool ignoreHurt) : base(character)
        {
            IgnoreHurt = ignoreHurt;

            Health = (StandardProperty<float>)character.GetFloat("Health");
            Defend = character.GetFloat("Defend");
            Dead   = character.GetBoolean("Dead");
            Hurt   = character.GetBoolean("Hurt");
        }

        public bool IgnoreHurt { get; }

        private Subject<HealthModel.Result> _Subject = new();

        public StandardProperty<float> Health { get; }

        public Property<float> Defend { get; }
        public Property<bool>  Dead   { get; }
        public Property<bool>  Hurt   { get; }

        public float Normalized => Health.Value / Health.Standard;

        public bool IsDead   => Dead.Value;
        public bool IsHurt   => Hurt.Value;
        public bool InActive => IsDead || IsHurt;

        public IDisposable Subscribe(IObserver<HealthModel.Result> observer) 
        {
            return _Subject.Subscribe(observer);
        }

        public override float Update(float damage)
        {
            if (IsDead) { return 0f; }

            var injure = (damage / (1f + Defend.Value * 0.01f)).Clamp(0, Health.Value);
            var health = (Health.Value - injure);

            Health.Set(health);

            if (Health.Value <= 0)
            {
                Dead.Set(true);

                Character.Rigidbody.isKinematic = true;
            }

            if (!IgnoreHurt) 
            {
                Hurt.Set(true);
            }

            _Subject.OnNext(new(Health.Standard, Health.Value, injure));

            return injure;
        }

        public struct Result 
        {
            public Result(float standard, float current, float delta) 
            {
                Standard = standard;
                Current  = current;
                Delta    = delta;

                Normalized      = Current / Standard;
                DeltaNormalized = Delta   / Standard;
            }

            public float Standard { get; }
            public float Current  { get; }
            public float Delta    { get; }
            
            public float Normalized      { get; }
            public float DeltaNormalized { get; }
        }
    }
}
