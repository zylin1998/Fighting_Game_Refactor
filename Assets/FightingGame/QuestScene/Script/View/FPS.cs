using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace FightingGame.QuestScene
{
    public class FPS : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI _Text;

        private void Update()
        {
            var deltaTime = Time.unscaledDeltaTime;
            var fps       = deltaTime > 0 ? deltaTime : 0f;
            _Text.SetText("FPS:" + (1f / fps).ToString("0"));
        }
    }
}
