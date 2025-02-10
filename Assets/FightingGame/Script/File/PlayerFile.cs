using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using Loyufei;
using Loyufei.ItemManagement;

namespace FightingGame
{
    [Serializable]
    public class PlayerFile : IAdjustableSaveable<ITradeLog>, IAdjustableSaveable<IItemStorage>, IAdjustableSaveable<IQuestLog>
    {
        [SerializeField]
        private List<Structure<ItemStorage>> _ItemStorages = new();
        [SerializeField]
        private List<Structure<TradeLog>>    _TradeLogs    = new();
        [SerializeField]
        private List<Structure<QuestLog>>    _QuestLogs    = new();

        public IItemStorage GetOrAdd(object id, Func<IItemStorage> add) 
        {
            var result = _ItemStorages.FirstOrDefault(s => s.Hash.Equals(id.GetHashCode()));
            
            if (result == null) 
            {
                result = new(id, (ItemStorage)add.Invoke());

                _ItemStorages.Add(result);
            }
            
            return result.Data;
        }

        public ITradeLog GetOrAdd(object id, Func<ITradeLog> add)
        {
            var result = _TradeLogs.Find(s => s.Hash.Equals(id.GetHashCode()));

            if (result == null)
            {
                result = new(id, (TradeLog)add.Invoke());

                _TradeLogs.Add(result);
            }

            return result.Data;
        }

        public IQuestLog GetOrAdd(object id, Func<IQuestLog> add)
        {
            var result = _QuestLogs.Find(s => s.Hash.Equals(id.GetHashCode()));
            
            if (result == null)
            {
                result = new(id, (QuestLog)add.Invoke());

                _QuestLogs.Add(result);
            }

            return result.Data;
        }
    }

    [Serializable]
    public class Structure<T>
    {
        public Structure(object id, T data)
        {
            _Hash = id.GetHashCode();

            _Data = data;
        }

        [SerializeField]
        private int _Hash;
        [SerializeField]
        private T   _Data;

        public int Hash => _Hash;
        public T   Data => _Data;
    }
}
