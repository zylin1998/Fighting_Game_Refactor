using System.Collections;
using System.Collections.Generic;

namespace FightingGame.TitleScene
{
    internal class BackMenu : FadeOutView
    {
        public override object ViewId => GroupUI.Back;

        public override IEnumerator ChangeState(bool isOn)
        {
            yield return null;

            gameObject.SetActive(isOn);
        }
    }
}