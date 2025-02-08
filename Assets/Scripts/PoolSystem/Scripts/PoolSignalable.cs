using System;
using Enums;
using Extentions;
using Managers;
using UnityEngine;
using UnityEngine.Events;

namespace Signalable
{
    public class PoolSignalable : MonoSingleton<PoolSignalable>
    {
        public UnityAction<BotType, BotManager> onAddBotToPool = delegate {  };
        public Func<BotType,BotManager> onGetBotFromPool = delegate { return null;};
    }
}