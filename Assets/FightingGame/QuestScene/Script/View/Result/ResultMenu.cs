using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;
using Loyufei.MVP;

namespace FightingGame.QuestScene
{
    internal class ResultMenu : FadeOutView
    {
        [SerializeField]
        private TextMeshProUGUI _Result;
        [SerializeField]
        private TextMeshProUGUI _PassTimeTitle;
        [SerializeField]
        private TextMeshProUGUI _PassTime;
        [SerializeField]
        private TextMeshProUGUI _DamageTitle;
        [SerializeField]
        private TextMeshProUGUI _Damage;
        [SerializeField]
        private TextMeshProUGUI _Gathered;
        [SerializeField]
        private Transform       _Content;
        [SerializeField]
        private Image           _Icon;

        [Inject]
        private DataRefresher Filter 
        {
            set 
            {
                value.Register("10013", (obj) => _PassTimeTitle.SetText(obj.ToString()));
                value.Register("10014", (obj) => _DamageTitle  .SetText(obj.ToString()));
            } 
        }

        [Inject]
        public ListenerPool    ListenerPool { get; }
        [Inject]
        public CurrencyMonitor Currency     { get; }
        [Inject]
        public QuestModel      QuestModel   { get; }

        private DynamicPoster _Next;

        public override object ViewId => GroupUI.Result;

        public override ILayout Layout()
        {
            var ids = new[] { "10010", "10011", "10015" };

            foreach (var id in ids) 
            {
                var listener = ListenerPool.Spawn("Button_Listener");

                listener.transform.SetParent(_Content);
                
                listener.EventId = id;
                listener.GroupId = Group.UI;

                if (id == "10015") 
                {
                    _Next = listener;

                    _Next.gameObject.SetActive(QuestModel.HasNext()); 
                }
            }

            _Icon.sprite = Currency.Get((object)20001).Item.Icon;

            return base.Layout();
        }

        public void Set(GameResult result) 
        {
            _Result  .SetText(result.Result ? "FULFFILL" : "DEFEAT");
            _PassTime.SetText(string.Format("{0}:{1}", result.PassTime.Minutes, result.PassTime.Seconds.ToString("00")));
            _Damage  .SetText(result.Injured.ToString("0.00"));
            _Gathered.SetText((result.Result ? result.Gather : 0).ToString());

            if (!result.Result) { _Next.gameObject.SetActive(false); }
        }
    }
}
