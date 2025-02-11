using System;
using System.Collections.Generic;
using Enums;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class TowerSignals : MonoSingleton<TowerSignals>
    {

        public UnityAction<Vector3> onHitTarget = delegate { };
        public UnityAction<int, string> onGetExp = delegate { };

    }
}