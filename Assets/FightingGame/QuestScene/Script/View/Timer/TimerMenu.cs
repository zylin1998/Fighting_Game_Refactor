using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using TMPro;
using Loyufei.MVP;
using StateMachine;

namespace FightingGame.QuestScene
{
    internal class TimerMenu : MonoViewBase
    {
        [SerializeField]
        private TextMeshProUGUI _Time;

        public override object ViewId => GroupUI.Timer;

        public IStateMachine Machine { get; private set; }

        public IObservable<float> CountDown(float second) 
        {
            var subject = new Subject<float>();
            
            Observable
                .EveryFixedUpdate()
                .TakeWhile((f) => second >= -1f)
                .Subscribe((f) => subject.OnNext(second -= Time.fixedDeltaTime), subject.OnError, subject.OnCompleted);

            subject.Subscribe(SetTime);

            return subject;
        }

        private void SetTime(float time) 
        {
            var result = Mathf.CeilToInt(time);

            _Time.SetText(result > 0 ? result.ToString() : "START!");
        }

        public override IEnumerator ChangeState(bool isOn)
        {
            yield return 1f;
            
            gameObject.SetActive(isOn);
        }
    }
}
