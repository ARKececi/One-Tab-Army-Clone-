using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotTargetFieldPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [FormerlySerializedAs("enemyAnimationController")] [SerializeField] private BotAnimationController botAnimationController;
        [SerializeField] private BotController _botController;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag(_botController.teamTag) && !other.CompareTag("Untagged"))
            {
                botAnimationController.Fight();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!other.CompareTag(_botController.teamTag) && !other.CompareTag("Untagged") && other.CompareTag("Dead"))
            {
                botAnimationController.Idle();
            }
        }
    }
}