using System;
using Enums;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotBodyPhysicsController : MonoBehaviour
    {
        #region Self Variables

        #region Serialized Variables

        [FormerlySerializedAs("enemyController")] [SerializeField] private BotController botController;
        [FormerlySerializedAs("_enemyAIController")] [SerializeField] private BotAIController botAIController;

        #endregion

        #endregion
    }
}