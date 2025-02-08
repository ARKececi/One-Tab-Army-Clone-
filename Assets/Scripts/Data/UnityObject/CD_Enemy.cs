using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Enemy", menuName = "Data/CD_Enemy", order = 0)]
    public class CD_Enemy : ScriptableObject
    {
        public SerializedDictionary<EnemyEnum, EnemyData> EnemyDatas;
    }
}