using System;
using System.Collections.Generic;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Signalable;
using Signals;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace Managers
{
    public class TowerManager : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public SerializedDictionary<BotType, PoolChange> PoolChanges;
        public Slider Slider;

        #endregion

        #region Serialized Variables

        [SerializeField] private BotFlockManager _botFlockManager;
        [SerializeField] private float countdownTime = 5f; // Sayaç süresi (saniye)
        [SerializeField] private GameObject _spawn;
        [SerializeField] private GameObject _alignment;
        [SerializeField] private Teams _teams;
        [SerializeField] private Material _material;
        [SerializeField] private Color _color;
        [SerializeField] private TextMeshProUGUI _textMeshProUGUI;
        

        #endregion
    
        #region Private Variables
        
        private bool collapse;
        private List<BotManager> spawnBots = new List<BotManager>();
        private int exp;
        private int _healt;
        private float timer;
        private int lwl;

        #endregion

        #endregion
        
        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            TowerSignals.Instance.onHitTarget += OnHitTarget;
            TowerSignals.Instance.onGetExp += TowerEXP;
        }

        private void UnsubscribeEvents()
        {
            TowerSignals.Instance.onHitTarget -= OnHitTarget;
            TowerSignals.Instance.onGetExp -= TowerEXP;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        private void Awake()
        {
            if (_textMeshProUGUI != null)
            {
                _textMeshProUGUI.text = $"{exp}/{10 + 5 * lwl}";
            }
            PoolChanges = new SerializedDictionary<BotType, PoolChange>();
            foreach (BotType botTypes in Enum.GetValues(typeof(BotType)))
            {
                PoolChanges.Add(botTypes, new PoolChange());
            }
        }

        public void TowerEXP(int BotEXP, string team)
        {
            if (_teams.ToString() != team) return;
            exp += BotEXP;
            if (_textMeshProUGUI == null) return;
            _textMeshProUGUI.text = $"{exp}/{10 + 5 * lwl}";
            if (exp >= 10 + (5 * lwl))
            {
                lwl++;
                exp = 0;
            }
        }

        public void HealtDamage(int damage)
        {
            _healt -= damage;
            SetHealt(_healt);
            if (_healt < 0)
            {
                
            }

            
        }
        
        public void SetHealt(float healt)
        {
            Slider.value = healt / 100;
        }
        
        public void OnHitTarget(Vector3 MouseHit)
        {
            if (!transform.CompareTag("Team1")) return;
            spawnBots.Clear();
            List<BotManager> useBotManager = new List<BotManager>();
            foreach (BotType VARIABLE in Enum.GetValues(typeof(BotType)))
            {
                foreach (BotManager botManager in GetUsedBotsByType(VARIABLE))
                {
                    useBotManager.Add(botManager);    
                }
                
            }
            AlignSpawnedBots(MouseHit, useBotManager,6,.4f);
        }

        
        private void Start()
        {
            StartTimer(countdownTime);
        }
        
        /// <summary>
        /// Botu pool'a ekler ve devre dışı bırakır.
        /// </summary>
        public void AddBotToPool(BotType botType, BotManager botManager)
        {
            botManager.gameObject.SetActive(false);
            // _botFlockManager.DeregisterBot(botManager.ReturnFlock()); //Çıkarıldı sürü sınıfı
            PoolChanges[botType].Pool.Add(botManager);
            botManager.BotReset();
        }
        
        /// <summary>
        /// Pool'dan bir bot alır. Eğer pool boşsa yeni bir bot oluşturur.
        /// </summary>
        public BotManager GetBotFromPool(BotType botType)
        {
            if (!PoolChanges.ContainsKey(botType)) return null;
            
            if (PoolChanges[botType].Pool.Count == 0)
            {
                BotManager newBot = PoolSignalable.Instance.onGetBotFromPool?.Invoke(botType);
                AddBotToPool(botType,newBot);
            }
            
            if (PoolChanges[botType].Pool.Count > 0)
            {
                var botManager = PoolChanges[botType].Pool[0];
                Material mat = new Material(_material);
                mat.color = _color;
                botManager.OnSetMaterial(mat);
                PoolChanges[botType].Pool.RemoveAt(0);
                PoolChanges[botType].Use.Add(botManager);
                botManager.transform.position = _spawn.transform.position;
                botManager.SetBotTag(_teams.ToString());
                botManager.TowerManager = this;
                botManager.gameObject.SetActive(true);
                return botManager;
            }
            return null;
        }

        #region Pool Helpers
        
        /// <summary>
        /// Belirtilen BotType'a ait useları nesnesini döndürür.
        /// </summary>
        public PoolChange GetPoolChangeByType(BotType botType)
        {
            return PoolChanges.ContainsKey(botType) ? PoolChanges[botType] : null;
        }
        
        /// <summary>
        /// Belirtilen BotType'a ait kullanılabilir botların listesini döndürür.
        /// </summary>
        public List<BotManager> GetAvailableBotsByType(BotType botType)
        {
            return PoolChanges.ContainsKey(botType) ? PoolChanges[botType].Pool : new List<BotManager>();
        }
        
        /// <summary>
        /// Belirtilen BotType'a ait şu an kullanımda olan botların listesini döndürür.
        /// </summary>
        public List<BotManager> GetUsedBotsByType(BotType botType)
        {
            return PoolChanges.ContainsKey(botType) ? PoolChanges[botType].Use : new List<BotManager>();
        }
        
        #endregion

        private void FixedUpdate()
        {
            SpawnTimer();
        }

        #region Spawn Timer
    
        /// <summary>
        /// Saldırı zamanlayıcısını başlatır.
        /// </summary>
        public void StartTimer(float duration)
        {
            timer = duration;
        }

        /// <summary>
        /// Saldırı zamanlayıcısını yönetir ve süre bitince sıfırlar.
        /// </summary>
        public void SpawnTimer()
        {
            timer -= Time.fixedDeltaTime;
            if (timer <= 0)
            {
                BotManager newBot = GetBotFromPool(BotType.SwordMan);
                newBot.OnHitTarget(_alignment.transform.position);
                AlignSpawnedBots(newBot, 5, .6f);
                StartTimer(countdownTime);
            }
        }

        #endregion

        #region Spawn Positioning
        
        /// <summary>
        /// Spawn edilen botları belirli bir düzen içinde hizalar.
        /// </summary>
        public void AlignSpawnedBots(Vector3 spawnPoint, List<BotManager> bots, int rowCount, float spacing)
        {
            if (bots == null || bots.Count == 0) return;

            int columnCount = Mathf.CeilToInt((float)bots.Count / rowCount);

            // Grid'in gerçek genişlik ve yüksekliği
            float totalWidth = (Mathf.Min(columnCount, bots.Count) - 1) * spacing;
            float totalDepth = (Mathf.Min(rowCount, Mathf.CeilToInt(bots.Count / (float)columnCount)) - 1) * spacing;

            // Grid'in merkezini tam olarak spawnPoint'e denk getirmek için kaydır
            Vector3 startPos = spawnPoint - new Vector3(totalWidth / 2f, 0, totalDepth / 2f);

            for (int i = 0; i < bots.Count; i++)
            {
                int row = i / columnCount;
                int col = i % columnCount;

                // Her botun konumu, başlangıç pozisyonuna göre belirlenir
                Vector3 newPos = startPos + new Vector3(col * spacing, 0, row * spacing);
                bots[i].OnHitTarget(newPos);
            }
        }
        #endregion
        
        /// <summary>
        /// Spawn edilen botları _alignment objesi etrafında hizalar.
        /// </summary>
        public void AlignSpawnedBots(BotManager bot, int rowCount, float spacing)
        {
            if (_alignment == null)
            {
                Debug.LogError("Alignment GameObject is not assigned!");
                return;
            }

            spawnBots.Add(bot); // Yeni botu listeye ekle

            int botIndex = spawnBots.Count - 1; // Yeni eklenen botun indexi
            int row = botIndex / rowCount; // Kaçıncı satırda olduğunu bul
            int col = botIndex % rowCount; // Kaçıncı sütunda olduğunu bul

            Vector3 basePosition = _alignment.transform.position; // Hizalama merkez noktası

            // Satırın ortasını belirle
            int centerIndex = rowCount / 2;

            // Pozisyon kaydırması için hesaplama (merkezden yayılma)
            int relativeIndex = col - centerIndex;
            int sign = (relativeIndex % 2 == 0) ? -1 : 1; // Çiftler sola, tekler sağa gidecek
            int step = (relativeIndex + 1) / 2; // Sıralamayı 3 - 2 - 4 - 1 - 5 gibi yapar

            float offset = sign * step * spacing; // Hesaplanan mesafeye göre kaydır

            // Yeni pozisyonu hesapla
            Vector3 newPos = basePosition + new Vector3(offset, 0, -row * spacing);

            // Botu belirlenen noktaya gönder
            DOVirtual.DelayedCall(0.1f, () => bot.OnHitTarget(newPos));
        }
    }
}
