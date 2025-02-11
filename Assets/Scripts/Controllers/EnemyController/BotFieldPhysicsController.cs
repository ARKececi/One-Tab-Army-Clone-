using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotFieldPhysicsController : MonoBehaviour
    {
        #region Serialized Variables

        #region Serialized Variables

        [SerializeField] private BotController _botController;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_botController.tag) && !other.CompareTag("Untagged") && other.TryGetComponent<BotManager>(out var enemyManager))
            {
                _botController.AddTarget(enemyManager);
            }

            if (!other.CompareTag(_botController.tag) && !other.CompareTag("Untagged") && other.TryGetComponent<TowerManager>(out var enemyTowerManager) )
            {
                _botController.EnemyTower(enemyTowerManager);
            }
        }
        
        private void OnTriggerExit(Collider other)
        {
            
            if (!other.CompareTag(_botController.tag) && !other.CompareTag("Untagged") && other.TryGetComponent<BotManager>(out var enemyManager))
            {
                _botController.RemoveTarget(enemyManager);
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Dead") && other.TryGetComponent<BotManager>(out var deadManager))
            {
                _botController.TargetDead(deadManager);
            }
        }
    }
}