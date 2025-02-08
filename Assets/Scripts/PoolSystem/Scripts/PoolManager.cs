using Controllers;
using Controllers.PoolController;
using Enums;
using Signalable;
using Signals;
using UnityEngine;

namespace Managers
{
    public class PoolManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [SerializeField] private PoolController poolController; 
        
        #endregion

        #endregion
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            PoolSignalable.Instance.onAddBotToPool += OnAddBotToPool;
            PoolSignalable.Instance.onGetBotFromPool += OnGetBotFromPool;
        }

        private void UnsubscribeEvents()
        {
            PoolSignalable.Instance.onAddBotToPool -= OnAddBotToPool;
            PoolSignalable.Instance.onGetBotFromPool -= OnGetBotFromPool;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        
        #endregion

        public void OnAddBotToPool(BotType botType, BotManager botManagerÜ)
        {
            poolController.AddBotToPool(botType,botManagerÜ);
        }

        public BotManager OnGetBotFromPool(BotType botType)
        {
            return poolController.GetBotFromPool(botType);
        }
    }
}