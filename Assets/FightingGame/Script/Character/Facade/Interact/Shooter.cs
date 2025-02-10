using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using Zenject;
using Loyufei;

namespace FightingGame
{
    public class Shooter : DynamicPoster
    {
        [SerializeField]
        private Transform _Point;

        public override object GroupId { get; set; } = Group.Gaming;

        private CoroutineDamageArea[] _Coroutines;

        private Queue<CoroutineDamageArea> _Await;

        private CoroutineDamageArea _Current = null;

        private IObservable<long> _Observable;

        private Vector3    Position => _Current?.transform.position ?? _Point.position;
        private Quaternion Rotation => _Current?.transform.rotation ?? _Point.rotation;

        [Inject]
        private void Construct(Character character) 
        {
            transform.SetParent(character.transform);

            gameObject.SetActive(false);
        }

        private void Awake()
        {
            gameObject.SetActive(false);
        }

        public void Set(params CoroutineDamageArea[] coroutines) 
        {
            _Coroutines = coroutines;
        }

        public void Set(IEnumerable<CoroutineDamageArea> coroutines) 
        {
            _Coroutines = coroutines.ToArray();
        }

        public void Initialize()
        {
            _Await = new Queue<CoroutineDamageArea>(_Coroutines);
        }

        public void Set(bool state) 
        {
            if (!state) { return; }

            if (!_Observable.IsDefault()) { return; }

            gameObject.SetActive(true);

            var coroutine = Coroutine();

            _Observable = Observable.EveryFixedUpdate().TakeWhile((l) => coroutine.MoveNext());

            _Observable.Subscribe((l) => { }, () => { _Observable = default; });
        }

        private IEnumerator Coroutine() 
        {
            while (gameObject.activeSelf)
            {
                var coroutine = _Current?.Coroutine() ?? false;
                
                if (!coroutine)
                {
                    if (!_Await.Any()) { break; }

                    var position = Position;
                    var rotation = Rotation;

                    _Current = _Await.Dequeue();

                    _Current.Set(true);
                    _Current.Set(Vector2.right);
                    _Current.Set(position, rotation);
                }

                yield return null;
            }

            _Current = default;

            gameObject.SetActive(false);
        }
    }
}
