using Loyufei;
using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

namespace FightingGame
{
    public interface IQuestLog : IGetter<bool>
    {
        public void Done(object id);
    }

    [Serializable]
    public class QuestLog : FlexibleRepositoryBase<int, bool>, IQuestLog
    {
        public void Done(object id) 
        {
            var r = SearchAt(id);

            if (r.IsDefault())
            {
                r = Create(1).First();

                r.Set(id, true);
            }
        }

        public bool Get(object id) 
        {
            return SearchAt(id)?.Data ?? false;
        }

        public bool Get(int index)
        {
            return SearchAt(index)?.Data ?? false;
        }

        public IEnumerable<bool> GetAll() 
        {
            return SearchAll().Select(r => r.Data);
        }
    }

}
