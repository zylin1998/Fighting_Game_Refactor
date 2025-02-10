using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zenject;
using TMPro;
using Loyufei.ItemManagement;

namespace FightingGame.TitleScene
{
    internal class TalentListener : DynamicPoster
    {
        [SerializeField]
        private Image           _Icon;
        [SerializeField]
        private TextMeshProUGUI _Title;
        [SerializeField]
        private TextMeshProUGUI _Purchase;
        [SerializeField]
        private TextMeshProUGUI _Count;
        [SerializeField]
        private Button          _Button;

        public TradeInfo Context { get; private set; }

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

        private object _EventId;

        public override object EventId
        {
            get => _EventId;

            set
            {
                if (_EventId == value) { return; }

                _EventId = value;
            }
        }

        private void Awake()
        {
            _Button.onClick.AddListener(OnButtonClick);

            Refresher.Register(Notes.Purchase, SetPurchase);
        }

        public void SetInfo(TradeInfo tradeInfo) 
        {
            Context = tradeInfo;

            _Icon.sprite = Context.Target.Item.Icon;

            Refresher.Unregister(SetContext);
            Refresher.Register(Context.Target.Id.ToString(), SetContext);

            Refresher.Unregister(SetInteractable);
            Refresher.Register(Context.Paid.Id, SetInteractable);
        }

        public void SetContext(object data) 
        {
            if (data is string str) { _Title.SetText(str.ToString()); }

            if (data is int count) { _Count.SetText(count.ToString()); }
        }

        public void SetInteractable(object data) 
        {
            if (data is int currency) 
            {
                _Button.interactable = currency >= Context.Paid.Count;
            }
        }

        public void SetPurchase(object data)
        {
            _Purchase.SetText(data.ToString());
        }

        private void OnButtonClick() 
        {
            Subject.OnNext(new Note(EventId, Context));
        }
    }
}
