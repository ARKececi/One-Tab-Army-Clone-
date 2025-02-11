using System;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotTargetFieldPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables
        
        [SerializeField] private BotController _botController;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_botController.tag) && !other.CompareTag("Untagged"))
            {
                _botController.Attack();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_botController.tag) && !other.CompareTag("Untagged"))
            {
                _botController.NotAttack();
            }
        }
    }
}