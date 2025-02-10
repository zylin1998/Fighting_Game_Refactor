using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using Zenject;

namespace FightingGame
{
    public class PluralShooter : DynamicPoster
    {
        [SerializeField]
        private List<Transform> _Points;
        [SerializeField]
        private float _Angle = -90f;

        protected Wave[] Waves { get; private set; }

        private IObservable<long> _Observable = null;

        [Inject]
        private void Construct(Character character, [Inject(Id = "Datum")] Transform parent) 
        {
            transform.SetParent(parent);

            gameObject.SetActive(false);
        }

        public void Set(IEnumerable<IEnumerable<CoroutineDamageArea>> coroutines) 
        {
            var index = 0;

            Waves = coroutines.Select(c => new Wave(index, _Angle, _Points[index++], c)).ToArray();
        }

        public void Set(int value) 
        {
            if (!gameObject.activeSelf) { gameObject.SetActive(true); }
            
            var result = value % 2;

            foreach (var wave in Waves) 
            {
                if (wave.Id == result) { wave.Initialize(); }
            }

            if (_Observable != null) { return; }

            var coroutine = Coroutine();

            _Observable = Observable.EveryFixedUpdate().TakeWhile(l => coroutine.MoveNext());

            _Observable.Subscribe((l) => { }, () => { _Observable = default; });
        }

        public IEnumerator Coroutine() 
        {
            var await = true;
            
            while (await) 
            {
                await = false;

                foreach (var wave in Waves) 
                {
                    if (wave.Coroutine()) { await = true; }
                }
                
                yield return null;
            }

            gameObject.SetActive(false);
        }

        protected class Wave 
        {
            public Wave(int id, float angle, Transform point, IEnumerable<CoroutineDamageArea> coroutines)
            {
                _Point = point;
                _Angle = angle;

                Id = id % 2;

                Coroutines = coroutines.ToArray();
            }

            public int Id { get; }

            [SerializeField]
            private Transform _Point;
            [SerializeField]
            private float     _Angle;

            private bool _Active;

            private CoroutineDamageArea[] Coroutines { get; }

            private Queue<CoroutineDamageArea> _Await;

            private CoroutineDamageArea _Current = null;

            private Vector3 Position => _Current?.transform.position ?? _Point.position;
            private float   Rotation => _Angle;

            public void Initialize() 
            {
                _Await = new Queue<CoroutineDamageArea>(Coroutines);

                _Current = null;

                _Active  = true;
            }

            public bool Coroutine() 
            {
                if (!_Active) { return false; }

                var coroutine = _Current?.Coroutine() ?? false;
                
                if (!coroutine)
                {
                    if (!_Await.Any()) { return (_Active = false); }

                    var position = Position;
                    var rotation = Rotation;

                    _Current = _Await.Dequeue();

                    _Current.Set(true);
                    _Current.Set(Vector2.down, 1f);
                    _Current.Set(position, true);
                    _Current.Set(_Angle);
                    
                    return _Current.Coroutine();
                }

                return coroutine;
            }
        }
    }
}
