using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotAIPhysics : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [FormerlySerializedAs("_enemyAIController")] [SerializeField] private BotAIController botAIController;

        #endregion

        #endregion

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Team1"))
            {
                botAIController.NullTarget();
            }
        }
    }
}