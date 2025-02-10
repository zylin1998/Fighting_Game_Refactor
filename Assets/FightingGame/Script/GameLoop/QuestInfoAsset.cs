using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;
using System.Runtime.CompilerServices;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "QuestInfoAsset", menuName = "FightingGame/Quest/InfoAsset", order = 1)]
    public class QuestInfoAsset : ScriptableObject
    {
        [SerializeField]
        private List<QuestInfo> _Infos;
        
        public QuestInfo this[int id] 
            => _Infos.FirstOrDefault(info => info.Id == id);

        public bool HasNext(int index)
        {
            return _Infos.Any(info => info.Id == index + 1);
        }

        public bool HasQuest(int index) 
        {
            return _Infos.Any(info => info.Id == index);
        }

        public IEnumerable<int> GetAllId() 
        {
            return _Infos.Select(info => info.Id);
        }
    }
}
