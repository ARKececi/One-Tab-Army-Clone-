using System;
using System.Collections.Generic;
using Managers;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotAIController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        public NavMeshAgent _agent;
        #endregion

        #region Serialized Variables
        [FormerlySerializedAs("enemyAnimationController")] [SerializeField] private BotAnimationController botAnimationController;
        [SerializeField] private BotController _botController;
        #endregion

        #region Private Variables
        [SerializeField] private List<Transform> targetList = new List<Transform>();
        private float distanceStop;
        private int speed;
        private bool isRunning;
        private Transform target;
        private Transform tower;
        #endregion
        
        #endregion

        private void Start()
        {
            _agent.speed = speed;
            distanceStop = _agent.stoppingDistance;
            botAnimationController.Idle();
            target = transform;
        }

        public void OnSpeed(int Speed)
        {
            speed = Speed;
        }

        public void EnemyTower(Transform transform)
        {
            tower = transform;
        }
        
        public void HitTarget(Transform hitTransform)
        {
            if (targetList.Count != 0) return;
            
            target = hitTransform;
            _agent.stoppingDistance = 0;
            StartRunning();
        }

        public void AddTarget(Transform enemyTarget)
        {
            _agent.stoppingDistance = distanceStop;
            targetList.Add(enemyTarget);
            target = targetList[0];
        }

        public void RemoveTarget(Transform enemyTarget)
        {
            targetList.Remove(enemyTarget);
        }

        public void NullTarget()
        {
            target = transform;
            StopRunning();
        }

        private void Update()
        {
            _agent.destination = target.position;
            if (_agent.remainingDistance <= distanceStop && targetList.Count != 0)
            {
                botAnimationController.Fight();
                _botController.AtackTimer();
            }
            else if (tower != null && _agent.remainingDistance <= distanceStop)
            {
                botAnimationController.Fight();
                _botController.AtackTimer();
            }
            else if (targetList.Count != 0 || tower != null) StartRunning();
        }

        private void StartRunning()
        {
            botAnimationController.Run();
        }

        private void StopRunning()
        {
            botAnimationController.Idle();
        }

        public void BotReset()
        {
            target = transform;
            botAnimationController.Idle();
            tower = null;
            distanceStop = 0;
            targetList.Clear();
            NullTarget();
        }
    }
}
