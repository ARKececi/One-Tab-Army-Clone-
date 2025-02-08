using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;

namespace Data.ValueObject
{
    [Serializable]
    public class PoolChange
    {
        public List<BotManager> Pool = new List<BotManager>();
        public List<BotManager> Use = new List<BotManager>();
    }
}