using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;
using Zenject;

namespace FightingGame
{
    public class CoroutineDamageArea : DamageArea
    {
        [SerializeField]
        protected Collider2D  _Collider;
        [SerializeField]
        protected Rigidbody2D _Rigidbody;

        public Vector2 Direct { get; protected set; }

        public float Side { get; set; }

        protected override void Awake()
        {
            _Collider.OnTriggerEnter2DAsObservable()
                .Where(c => c.tag != tag && c.tag != "Ignore")
                .Subscribe(OnCollision);

            Set(false);
        }

        public void Set(Vector3 position, bool ignoreScale = false)
        {
            Set(position, transform.rotation, ignoreScale);
        }

        public void Set(Vector3 position, Quaternion rotation, bool ignoreScale = false) 
        {
            transform.SetPositionAndRotation(position, rotation);

            if (!ignoreScale)
            {
                var side = Character.transform.localScale.x > 0 ? 1f : -1f;

                var scale = transform.localScale;
                var result = new Vector3(Mathf.Abs(scale.x) * side, scale.y, scale.z);

                transform.localScale = result;
            }
        }

        public void Set(float rotation)
        {
            if (_Rigidbody.constraints.HasFlag(RigidbodyConstraints2D.FreezeRotation)) { return; }

            _Rigidbody.rotation = rotation;
        }

        public void Set(Vector2 direct, float side = 0) 
        {
            Direct = direct;

            if (side == 0) { return; }

            Side = side;
        }

        protected override void Construct(Character character, AudioModel audio)
        {
            Character = character;

            _Damage = Character.GetModel<DamageCalculateModel>();

            Audio = audio;

            Clip = Character.GetAsset<AudioClip>(Character.name + "_Attack_Audio");
        }

        [Inject]
        protected void Construct([Inject(Id = "Datum")]Transform parent) 
        {
            transform.SetParent(parent);
        }

        public virtual bool Coroutine() 
        {
            return gameObject.activeSelf;
        }
    }
}
