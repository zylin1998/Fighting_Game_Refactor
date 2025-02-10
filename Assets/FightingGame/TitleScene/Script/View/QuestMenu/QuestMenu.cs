using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.TitleScene
{
    internal class QuestMenu : FadeOutView
    {
        [SerializeField]
        private Transform _QuestContent;
        [SerializeField]
        private Transform _BackContent;
        
        public override object ViewId => GroupUI.Quest;

        [Inject]
        public ListenerPool ListenerPool { get; }
        [Inject]
        public QuestModel   QuestModel { get; }

        public override ILayout Layout()
        {
            var isDone  = QuestModel.IsDone().ToArray();
            var preview = true;

            for (var i = 1; i <= isDone.Length; i++) 
            {
                var listener = ListenerPool.Spawn("Quest_Listener") as QuestListener;

                listener.transform.SetParent(_QuestContent);
                listener.EventId = Notes.Quest;
                listener.Content = i;
                listener.GroupId = Group.UI;

                var done = isDone[i - 1];

                listener.Set(done, preview);

                if (!done) { preview = false; }
            }

            var back = ListenerPool.Spawn("Button_Listener");

            back.transform.SetParent(_BackContent);
            back.EventId = Notes.Back;
            back.GroupId = Group.UI;

            return base.Layout();
        }
    }
}
