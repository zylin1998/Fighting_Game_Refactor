using Loyufei.DomainEvents;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FightingGame
{
    public class IntegerListener : DynamicPoster
    {
        [SerializeField]
        private TextMeshProUGUI _Title;
        [SerializeField]
        private TextMeshProUGUI _Context;
        [SerializeField]
        private Button          _Button;

        public override object GroupId { get; set; } = Group.System;

        public int Context { get; private set; }

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

                Refresher.Unregister(SetTitle);
                Refresher.Unregister(SetContext);

                _EventId = value;

                Refresher.Register(_EventId, SetTitle);
                Refresher.Register(_EventId + "1", SetContext);
            }
        }

        private void Awake()
        {
            _Button.onClick.AddListener(ButtonEvent);
        }

        private void SetTitle(object obj) 
        {
            _Title.SetText(obj.ToString());
        }

        private void SetContext(object obj)
        {
            if (obj is int context) { Context = context; }

            else { _Context.SetText(obj.ToString()); }
        }

        private void ButtonEvent() 
        {
            Context++;

            Subject.OnNext(new Note(EventId, Context));
        }
    }
}
