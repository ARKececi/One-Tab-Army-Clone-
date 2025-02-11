using System.Collections.Generic;
using Extentions;
using UnityEngine;
using UnityEngine.Events;

namespace Signals
{
    public class GameSignals : MonoSingleton<GameSignals>
    {
        public UnityAction<string> onGameLoseOrWin = delegate { };
        public UnityAction<string> onTeamTag = delegate { };
        public UnityAction<bool> onGamePause = delegate { };
    }
}