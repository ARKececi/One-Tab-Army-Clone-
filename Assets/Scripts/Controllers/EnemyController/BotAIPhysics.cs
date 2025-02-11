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

        #region Private Variables

        public string ıd;

        #endregion

        #endregion
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.name == ıd)
            {
                botAIController.NullTarget();
            }
        }
    }
}