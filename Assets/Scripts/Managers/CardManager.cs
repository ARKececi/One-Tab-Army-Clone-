using System;
using System.Collections.Generic;
using Data.UnityObject;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Signals;
using UnityEngine.Rendering;

public class CardManager : MonoBehaviour
{
    [Header("Kart Verileri")]
    [SerializeField] private SerializedDictionary<CardType, List<GameObject>> cardDictionary; // Her kart türü için farklı seviyelerde kartları tutan sözlük
    [SerializeField] private SerializedDictionary<CardType, int> cardLevels; // Kartların seviyelerini saklayan sözlük

    [Header("UI Elemanları")]
    [SerializeField] private GameObject cardPanel; // Kart seçim ekranını temsil eden panel
    [SerializeField] private Transform cardHolder; // Kartların yerleşeceği UI alanı

    [Header("Durum Kontrolleri")]
    [SerializeField] private bool isCardSelectionActive = false; // Eğer true olursa kart seçim ekranı açılır
    
    private void OnEnable()
    {
        SubscribeEvents();
    }

    private void SubscribeEvents()
    {
        CardSignals.Instance.onNextLevel += OnNextLevel;
    }

    private void UnsubscribeEvents()
    {
        CardSignals.Instance.onNextLevel -= OnNextLevel;
    }

    private void OnDisable()
    {
        UnsubscribeEvents();
    }

    private void Awake()
    {
        OnGetData();
    }

    private void OnGetData()
    {
        cardDictionary = Resources.Load<CD_Card>("Data/CD_Card").CardData.CardDictionary; 
    }
    
    private void OnNextLevel()
    {
        // Eğer kart seçimi aktifse, oyunu durdur ve seçim ekranını aç
        if (isCardSelectionActive)
        {
            GameSignals.Instance.onGamePause?.Invoke(true);
            ShowCardSelection(); // Kart seçim ekranını göster
            isCardSelectionActive = false; // Tekrar çağırmamak için flag'i kapat
        }
    }

    /// <summary>
    /// Kart seçim ekranını açar ve 3 rastgele kart gösterir
    /// </summary>
    public void ShowCardSelection()
    {
        cardPanel.SetActive(true); // Kart panelini aç
        List<GameObject> selectedCards = GetRandomUniqueCards(3); // 3 farklı türden kart seç

        // Önceki kartları temizle
        foreach (Transform child in cardHolder)
        {
            Destroy(child.gameObject);
        }

        // Kartların düşüş animasyonu için başlangıç değerleri
        float startY = 800f; // Kartlar başlangıçta yukarıdan gelecek
        float delayBetweenCards = 0.2f; // Kartların sırayla düşmesi için gecikme süresi

        for (int i = 0; i < selectedCards.Count; i++)
        {
            // Yeni kartı oluştur ve holder içine yerleştir
            GameObject cardPrefabInstance = Instantiate(selectedCards[i], cardHolder);
            RectTransform rect = cardPrefabInstance.GetComponent<RectTransform>();

            // Kartın başlangıç pozisyonunu yukarıya ayarla
            Vector3 startPos = rect.anchoredPosition;
            rect.anchoredPosition = new Vector3(startPos.x, startY);

            // Kartı yumuşak bir şekilde aşağı indir
            rect.DOAnchorPosY(startPos.y, 0.5f)
                .SetEase(Ease.OutBounce)
                .SetDelay(i * delayBetweenCards);

            // Kartın butonuna tıklanınca çağrılacak fonksiyonu ata
            Button button = cardPrefabInstance.GetComponent<Button>();
            CardType cardType = GetCardTypeFromPrefab(cardPrefabInstance);
            button.onClick.AddListener(() => SelectCard(cardType));
        }
    }

    /// <summary>
    /// Belirtilen sayıda benzersiz kart seçer (her biri farklı türde olur)
    /// </summary>
    private List<GameObject> GetRandomUniqueCards(int count)
    {
        List<CardType> availableTypes = new List<CardType>(cardDictionary.Keys); // Mevcut kart türlerini al
        List<GameObject> selectedCards = new List<GameObject>();

        while (selectedCards.Count < count && availableTypes.Count > 0)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableTypes.Count);
            CardType selectedType = availableTypes[randomIndex];
            availableTypes.RemoveAt(randomIndex); // Aynı türden bir daha seçmemesi için listeden çıkar

            int level = cardLevels[selectedType]; // Kartın mevcut seviyesini al
            GameObject card = cardDictionary[selectedType][Mathf.Min(level, cardDictionary[selectedType].Count - 1)]; // Seviye sınırını aşmaz
            selectedCards.Add(card);
        }

        return selectedCards;
    }

    /// <summary>
    /// Kullanıcı bir kart seçtiğinde çağrılır, kartın seviyesini artırır ve seçim ekranını kapatır
    /// </summary>
    private void SelectCard(CardType selectedType)
    {
        cardLevels[selectedType]++; // Kartın seviyesini artır
        cardPanel.SetActive(false); // Kart seçim ekranını kapat
        GameSignals.Instance.onGamePause?.Invoke(false);
    }

    /// <summary>
    /// Kart nesnesinin hangi türe ait olduğunu belirler
    /// </summary>
    private CardType GetCardTypeFromPrefab(GameObject cardPrefab)
    {
        foreach (var entry in cardDictionary)
        {
            if (entry.Value.Contains(cardPrefab))
            {
                return entry.Key;
            }
        }
        return default;
    }
}
