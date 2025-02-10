using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UniRx;
using UnityEngine.UI;

namespace FightingGame.QuestScene
{
    internal class CoinGatherDisplay : MonoBehaviour
    {
        [SerializeField]
        private Image           _Icon;
        [SerializeField]
        private TextMeshProUGUI _Total;
        [SerializeField]
        private TextMeshProUGUI _Gathered;

        private float _Delay;
        private float _Coin;

        private float _DelayTime;

        private IObservable<long> _Await;

        public void Initialize()
        {
            SetGather(0);

            _Total.SetText(0.ToString());
        }

        public void Set(Sprite sprite) 
        {
            _Icon.sprite = sprite;
        }

        public void Set(int gather)
        {
            SetGather(gather);
            
            _DelayTime = 0.5f;

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
            _Coin  += _Delay;
            _Delay = 0;

            SetGather(0);

            _Total.SetText(_Coin.ToString());

            _Await = default;
        }

        private void SetGather(int gather) 
        {
            _Delay += gather;

            _Gathered.SetText("+" + gather);

            _Gathered.enabled = gather > 0;
        }
    }
}
