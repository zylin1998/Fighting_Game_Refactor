using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FightingGame
{
    public class FloatListener : DynamicPoster
    {
        [SerializeField]
        private TextMeshProUGUI _Title;
        [SerializeField]
        private Slider          _Slider;
        [SerializeField]
        private Toggle          _Toggle;

        public override object GroupId { get; set; } = Group.System;

        [Inject]
        public DataRefresher Refresher { get; }

        [Inject]
        public AudioModel Audio
        {
            set
            {
                value.BindSelectables(_Slider);
                value.BindSelectables(_Toggle);
            }
        }

        private object _EventId;

        public override object EventId
        {
            get => _EventId;

            set
            {
                if (_EventId == value) { return; }

                _EventId = value;

                Refresher.Unregister(SetText);
                Refresher.Unregister(SetSlider);
                Refresher.Unregister(SetToggle);

                Refresher.Register(_EventId, SetText);
                Refresher.Register(_EventId + "1", SetSlider);
                Refresher.Register(_EventId + "2", SetToggle);
            }
        }

        private void SetText(object obj) 
        {
            _Title.SetText(obj.ToString());
        }

        private void SetSlider(object obj) 
        {
            if (obj is float value) { _Slider.SetValueWithoutNotify(value * 100); }
        }

        private void SetToggle(object obj)
        {
            if (obj is bool isOn) { _Toggle.SetIsOnWithoutNotify(isOn); }
        }

        private void Awake()
        {
            _Toggle.onValueChanged.AddListener(ToggleEvent);
            _Slider.onValueChanged.AddListener(SliderEvent);
        }

        private void SliderEvent(float value) 
        {
            Subject.OnNext(new Note(EventId, value / 100f));
        }

        private void ToggleEvent(bool isOn)
        {
            Subject.OnNext(new Note(EventId, isOn));
        }
    }
}
