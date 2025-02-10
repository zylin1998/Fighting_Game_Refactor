using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FightingGame
{
    [CreateAssetMenu(fileName = "QuestSelector", menuName = "FightingGame/Quest/QuestSelector", order = 1)]
    public class QuestSelector : ScriptableObject
    {
        [SerializeField, Min(1)]
        private int _QuestId;

        public int QuestId
        { 
            get => _QuestId; 
            
            set => _QuestId = value; 
        }
    }
}
