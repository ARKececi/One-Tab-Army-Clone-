using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace Data.ValueObject
{
    public class CardData
    {
        public SerializedDictionary<CardType, List<GameObject>> CardDictionary; // Her kart türü için farklı seviyelerde kartları tutan sözlük
    }
}