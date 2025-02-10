using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FightingGame
{
    public class ButtonListener : DynamicPoster
    {
        [SerializeField]
        private TextMeshProUGUI _Title;
        [SerializeField]
        private Button          _Button;

        public override object GroupId { get; set; } = Group.System;

        [Inject]
        public DataRefresher Refresher { get; }

        [Inject]
        public AudioModel Audio
        {
            set
            {
                value.BindSelectables(_Button);
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

                Refresher.Register(_EventId, SetText);
            }
        }

        private void Awake()
        {
            _Button.onClick.AddListener(ButtonEvent);
        }

        private void SetText(object obj) 
        {
            _Title.SetText(obj.ToString());
        }

        private void ButtonEvent()
        {
            Subject.OnNext(new Note(EventId, default));
        }
    }
}
