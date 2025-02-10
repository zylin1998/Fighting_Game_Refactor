using System;
using System.Linq;
using System.Collections.Generic;
using Zenject;

namespace FightingGame
{
    public class QuestModel : PropertyModel
    {
        public QuestModel(GlobalDataAccess dataAccess) 
        {
            dataAccess.Install(this);
        }

        [Inject(Id = "Quest")]
        public IQuestLog      QuestLog       { get; }
        [Inject]
        public QuestSelector  QuestSelector  { get; }
        [Inject]
        public QuestInfoAsset QuestInfoAsset { get; }

        public QuestInfo Current
            => QuestInfoAsset[QuestSelector.QuestId];

        public void Done() 
        {
            if (QuestInfoAsset.HasQuest(QuestSelector.QuestId)) 
            {
                QuestLog.Done(QuestSelector.QuestId);
            }
        }

        public bool GoTo(int index) 
        {
            var result = QuestInfoAsset.HasQuest(QuestSelector.QuestId);

            if (result) { QuestSelector.QuestId = index; }

            return result;
        }

        public bool GoNext() 
        {
            var result = QuestInfoAsset.HasNext(QuestSelector.QuestId);

            if (result) { QuestSelector.QuestId++; }

            return result;
        }

        public bool HasNext() 
        {
            return QuestInfoAsset.HasNext(QuestSelector.QuestId);
        }

        public IEnumerable<bool> IsDone() 
        {
            return QuestInfoAsset.GetAllId().Select(id => QuestLog.Get((object)id));
        }
    }
}
