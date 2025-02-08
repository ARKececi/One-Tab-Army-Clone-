using System;
using UnityEngine.Serialization;

namespace Data.ValueObject
{
    [Serializable]
    public class EnemyData
    {
        public int Healt;
        public int Damage;
        [FormerlySerializedAs("NormalSpeed")] public int Speed;
        public int FastSpeed;
    }
}