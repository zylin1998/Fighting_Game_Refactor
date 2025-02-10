using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace FightingGame.QuestScene
{
    internal class TimeDisplay : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _Time;
        [SerializeField]
        private string          _Format = "{0}:{1}";
        
        public void Set(TimeSpan time) 
        {
            _Time.SetText(string.Format(_Format, time.Minutes, time.Seconds.ToString("00")));
        }
    }
}
