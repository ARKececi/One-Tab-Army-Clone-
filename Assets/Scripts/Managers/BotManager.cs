using Controllers.EnemyController;
using Signals;
using UnityEngine;
using UnityEngine.Serialization;

namespace Managers
{
    public class BotManager : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [FormerlySerializedAs("enemyAIController")] [SerializeField] private BotAIController botAIController;
        [FormerlySerializedAs("enemyController")] [SerializeField] private BotController botController;

        #endregion

        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {
            EnemySignals.Instance.onHitTarget += OnHitTarget;
        }

        private void UnsubscribeEvents()
        {
            EnemySignals.Instance.onHitTarget -= OnHitTarget;
        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        public void OnNullTarget()
        {
            if (botAIController.enabled)
            {
                botAIController.NullTarget();
            }
        }

        public void OnHitTarget(Transform MouseHit)
        {
            botAIController.HitTarget(MouseHit);
        }

        public bool OnHitDamage(int damge)
        {
            return botController.HealtDamage(damge);
        }
        
    }
}