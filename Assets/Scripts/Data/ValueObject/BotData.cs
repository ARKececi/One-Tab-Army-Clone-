using System;
using System.Collections.Generic;
using UnityEngine.Serialization;

namespace Data.ValueObject
{
    [Serializable]
    public class BotData
    {
        [FormerlySerializedAs("Healt")] public int Health;
        public int Damage; 
        public int Speed;
        public List<(int,int)> Lwl;
    }
}