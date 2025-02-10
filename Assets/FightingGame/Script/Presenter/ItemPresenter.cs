using Loyufei;
using Loyufei.ItemManagement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class ItemPresenter : EventNotesPresenter
    {
        public override object GroupId => Group.Item;

        [Inject]
        public Trade           Trade           { get; }
        [Inject]
        public Currency        Currency        { get; }
        [Inject]
        public Talent          Talent          { get; }
        [Inject]
        public CurrencyMonitor CurrencyMonitor { get; }
        [Inject]
        public TalentMonitor   TalentMonitor   { get; }
        [Inject]
        public TradeMonitor    TradeMonitor    { get; }
        [Inject]
        public DataRefresher   Refresher       { get; }
        [Inject]
        public SaveSystemModel SaveModel       { get; }
        
        protected override void Init()
        {
            Add(Notes.TradeTalent, TradeTalent);
            Add(Notes.AddTalent  , AddTalent);
            Add(Notes.AddCurrency, AddCurrency);
            Add(Notes.Read       , RefreshAll);
        }

        public void TradeTalent(object data) 
        {
            var trade = (TradeInfo)data;

            var result = Trade.Purchase(trade.Identity, trade.Target.Count);

            if (result.overflow > 0) { Debug.Log("Purchase Overflow"); }

            Currency.Remove(result.info.Paid.Id, result.preserve * result.info.Paid.Count);
            Talent  .Add(result.info.Target.Id, result.preserve * result.info.Target.Count);

            SaveModel.Save("GameFile");

            Refresh(trade);
        }

        private void AddTalent(object data)
        {
            var add = (Add)data;
            
            Talent.Add(add.Id, add.Count);

            SaveModel.Save("GameFile");
        }

        private void AddCurrency(object data) 
        {
            var add = (Add)data;

            var overflow = Currency.Add(add.Id, add.Count);

            SaveModel.Save("GameFile");
        }

        private void Refresh(TradeInfo info) 
        {
            var talent   = TalentMonitor  .Get(info.Target.Id).Count;
            var currency = CurrencyMonitor.Get(info.Paid  .Id).Count;
            
            Refresher.Refresh(info.Target.Id.ToString(), talent);
            Refresher.Refresh(info.Paid  .Id, currency);
        }

        private void RefreshAll(object data)
        {
            foreach (var info in TradeMonitor.GetAll()) 
            {
                Refresh(info.info);
            }
        }
    }

    public struct Add 
    {
        public Add(object id, int count) 
        {
            Id    = id;
            Count = count;
        }

        public object Id    { get; }
        public int    Count { get; }
    }

    public struct TradePremit 
    {
        public TradePremit(int count, bool premit) 
        {
            Count  = count;
            Premit = premit;
        }

        public int  Count  { get; }
        public bool Premit { get; }
    }
}
