using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;

namespace FightingGame.TitleScene
{
    internal class QuestListener : DynamicPoster
    {
        [SerializeField]
        private TextMeshProUGUI _Title;
        [SerializeField]
        private Button          _Button;
        [SerializeField]
        private Image           _Done;

        [Inject]
        public DataRefresher Refresher { get; }
        [Inject]
        public AudioModel    Audio 
        { 
            set 
            {
                value.BindSelectables(_Button);
            } 
        }

        public int Content { get; set; }

        private object _EventId;

        public override object EventId
        {
            get => _EventId;

            set
            {
                if (_EventId == value) { return; }

                _EventId = value;

                Refresher.Unregister(SetTitle);
                Refresher.Register(_EventId, SetTitle);
            }
        }

        public void Set(bool isDone, bool preview) 
        {
            _Done.enabled = isDone;

            gameObject.SetActive(isDone || preview);
        }

        private void Awake()
        {
            _Button.onClick.AddListener(ButtonEvent);
        }

        private void SetTitle(object obj)
        {
            _Title.SetText(string.Format("{0} - {1}", obj.ToString(), Content));
        }

        private void ButtonEvent()
        {
            Subject.OnNext(new Note(Notes.QuestSelect, Content));
        }
    }
}
