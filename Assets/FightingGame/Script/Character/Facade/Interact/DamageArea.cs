using Loyufei.DomainEvents;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class DamageArea : DynamicPoster, IObservable<IDomainEvent>
    {
        public Character Character { get; protected set; }

        protected DamageCalculateModel _Damage;

        public override object GroupId { get; set; } = Group.Gaming; 

        public AudioClip   Clip   { get; protected set; }
        public AudioModel  Audio  { get; protected set; }

        [Inject]
        protected virtual void Construct(Character character, AudioModel audio) 
        {
            Character = character;

            var parent = Character.transform;

            transform.SetParent(parent);
            transform.SetPositionAndRotation(parent.position, parent.rotation);
            var modelX = Character.Model.localScale.x >= 0 ? 1f : -1f;
            var localScale = transform.localScale;
            localScale.x *= modelX;
            transform.localScale = localScale;

            _Damage = Character.GetModel<DamageCalculateModel>();

            Audio = audio;

            Clip = Character.GetAsset<AudioClip>(Character.name + "_Attack_Audio");
        }

        protected virtual void Awake()
        {
            this.OnTriggerEnter2DAsObservable()
                .Where(c => c.tag != tag && c.tag != "Ignore")
                .Subscribe(OnCollision);

            Set(false);
        }

        protected virtual void OnCollision(Collider2D collider) 
        {
            TakeDamage(collider);
        }

        protected void TakeDamage(Collider2D collider) 
        {
            var character = collider.GetComponent<Character>();

            if (character) 
            {
                var distance = transform.position - character.Position;

                var damage = new TakeDamage(character, _Damage, distance.x > 0 ? -1f : 1f);

                Subject.OnNext(new Note(Notes.TaleDamage, damage));

                Audio.Play("SFX", Clip);
            }
        }

        public virtual void Set(bool active) 
        {
            if (active == gameObject.activeSelf) { return; }

            gameObject.SetActive(active);
        }
    }

    public struct TakeDamage 
    {
        public TakeDamage(Character character, DamageCalculateModel damage, float force) 
        {
            Tag    = character.tag;
            GUID   = character.GUID;
            Damage = damage.CalculatedDamage;
            Force  = new (force, 0);
        }

        public string  Tag    { get; }
        public int     GUID   { get; }
        public float   Damage { get; }
        public Vector2 Force  { get; }
    }
}
