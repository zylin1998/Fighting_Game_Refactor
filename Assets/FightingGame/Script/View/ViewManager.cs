using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace FightingGame
{
    public class ViewManager : Loyufei.MVP.ViewManager
    {
        [Inject]
        public ViewManager() : base() 
        {

        }
    }
}
