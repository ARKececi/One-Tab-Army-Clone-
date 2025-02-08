using System;
using UnityEngine.Serialization;

namespace Data.ValueObject
{
    [Serializable]
    public class BotData
    {
        public int Healt;
        public int Damage; 
        public int Speed;
    }
}