using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.Rendering;

namespace Controllers.PoolController
{
    public class PoolController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables

        public SerializedDictionary<BotType, PoolData> PoolData;
        public SerializedDictionary<BotType, PoolChange> PoolChanges;
        #endregion

        #region Serialized Variables
        [SerializeField] private GameObject place;
        #endregion

        #endregion
        
        private void Awake()
        {
            PoolData = GetBotData();
            PoolChanges = new SerializedDictionary<BotType, PoolChange>();
            
            foreach (var botType in PoolData.Keys)
            {
                PoolChanges.Add(botType, new PoolChange());
            }

            Pooling();
        }
        
        private SerializedDictionary<BotType, PoolData> GetBotData()
        {
            return Resources.Load<CD_Pool>("Data/CD_Pool").PoolDatas;
        }
        
        private void Pooling()
        {
            foreach (var botType in PoolData.Keys)
            {
                int botCount = PoolData[botType].PoolCount;
                for (int i = 0; i < botCount; i++)
                {
                    CreateBotInstance(botType);
                }
            }
        }

        private void CreateBotInstance(BotType botType)
        {
            var botInstance = Instantiate(PoolData[botType].PoolObj, place.transform);
            var botManager = botInstance.GetComponent<BotManager>();
            if (botManager != null)
            {
                AddBotToPool(botType, botManager);
            }
        }
        
        public void AddBotToPool(BotType botType, BotManager botManager)
        {
            botManager.gameObject.SetActive(false);
            PoolChanges[botType].Pool.Add(botManager);
        }
        
        public BotManager GetBotFromPool(BotType botType)
        {
            if (PoolChanges[botType].Pool.Count == 0)
            {
                CreateBotInstance(botType);
            }
            
            if (PoolChanges[botType].Pool.Count > 0)
            {
                var botManager = PoolChanges[botType].Pool[0];
                PoolChanges[botType].Pool.RemoveAt(0);
                PoolChanges[botType].Use.Add(botManager);
                return botManager;
            }
            return null;
        }
    }
}
