using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace FightingGame.QuestScene
{
    internal class PauseListener : DynamicPoster
    {
        [SerializeField]
        private Button _Button;

        [Inject]
        public AudioModel AudioModel
        {
            set 
            {
                value.BindSelectables(_Button);
            }
        }

        public override object GroupId { get; set; } = Group.UI;

        private void Awake()
        {
            _Button.onClick.AddListener(ButtonEvent);
        }

        private void ButtonEvent()
        {
            Subject.OnNext(new Note(EventId, default));
        }
    }
}
