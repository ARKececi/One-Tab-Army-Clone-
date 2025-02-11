using Data.ValueObject;
using UnityEngine;

namespace Data.UnityObject
{
    [CreateAssetMenu(fileName = "CD_Card", menuName = "Data/CD_Card", order = 0)]
    public class CD_Card : ScriptableObject
    {
        public CardData CardData;
    }
}