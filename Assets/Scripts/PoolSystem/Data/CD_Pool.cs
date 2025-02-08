using Data.ValueObject;
using Enums;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Pool", menuName = "Data/CD_Pool", order = 0)]
    public class CD_Pool : ScriptableObject
    {
        public SerializedDictionary<BotType, PoolData> PoolDatas;
    }
}