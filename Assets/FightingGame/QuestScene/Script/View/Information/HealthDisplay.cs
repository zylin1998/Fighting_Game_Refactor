using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace FightingGame.QuestScene
{
    internal class HealthDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image _Instance;
        [SerializeField]
        private Image _Delay;

        private float _DelayTime;

        public float Normalized { get; private set; }

        private IObservable<long> _Await;

        public void Initialize()
        {
            _Delay   .fillAmount = 1f;
            _Instance.fillAmount = 1f;
        }

        public void Set(float normalized) 
        {
            _DelayTime = 0.5f;

            Normalized = normalized;

            _Instance.fillAmount = Normalized;

            if (_Await != default) { return; }

            _Await = Observable.EveryFixedUpdate()
                .TakeWhile((l) => _DelayTime >= 0f);
            
            _Await.Subscribe(PassTime, DelayOver);
        }

        private void PassTime(long frameCount) 
        {
            _DelayTime -= Time.fixedDeltaTime;
        }

        private void DelayOver() 
        {
            _Delay.fillAmount = Normalized;
            
            _Await = default;
        }
    }
}
