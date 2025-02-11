using System;
using System.Collections.Generic;
using Managers;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Controllers.EnemyController
{
    public class BotAIController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        [FormerlySerializedAs("_agent")] public NavMeshAgent Agent;
        #endregion

        #region Serialized Variables
        [SerializeField] private BotAnimationController botAnimationController;
        [SerializeField] private BotController _botController;
        [SerializeField] private BotFlock _botFlock;
        [SerializeField] private BotAIPhysics _botAIPhysics;
        [SerializeField] private NavMeshObstacle _obstacle;
        #endregion

        #region Private Variables
        [SerializeField] private List<Transform> targetList = new List<Transform>();
        private float distanceStop;
        private int speed;
        private bool isRunning;
        private Transform target;
        private Transform tower;
        private bool isBotFlock;
        [SerializeField] private Collider hit;
        #endregion
        
        #endregion

        private void Start()
        {
            isRunning = true;
            Agent.speed = speed;
            distanceStop = Agent.stoppingDistance;
            botAnimationController.Idle();
            target = transform;
            Agent.avoidancePriority = 50;  // Çarpışma önceliğini orta seviyeye al
        }

        public void OnSpeed(int Speed)
        {
            speed = Speed;
        }

        public void EnemyTower(Transform transform)
        {
            tower = transform;
        }
        
        public void HitTarget(Vector3 hitTransform)
        {
            if (targetList.Count != 0 || tower != null) return;
            hit.transform.position = hitTransform;
            target = hit.transform;
            hit.gameObject.SetActive(true);
            hit.transform.SetParent(transform.parent);
            Agent.stoppingDistance = 0;
            _obstacle.enabled = false;
            Agent.enabled = true;
            StartRunning();
            Agent.SetDestination(target.position);
        }

        public void AddTarget(Transform enemyTarget)
        {
            Agent.stoppingDistance = .5f;
            targetList.Add(enemyTarget);
            hit.transform.SetParent(transform);
            hit.gameObject.SetActive(false);
            target = targetList[0];
            _obstacle.enabled = false;
            Agent.enabled = true;
            Agent.SetDestination(target.position);
        }

        public void RemoveTarget(Transform enemyTarget)
        {
            targetList.Remove(enemyTarget);
            if (targetList.Count == 0)
            {
                target = transform;
                StopRunning();
            }
            else
            {
                target = targetList[0];
            }
        }

        public void NullTarget()
        {
            if (targetList.Count != 0 || tower != null) return;
            target = transform;
            hit.transform.SetParent(transform);
            hit.gameObject.SetActive(false);
            StopRunning();
        }

        private void Update()
        {
            
            if (Agent.pathPending) return; // Hedef hesaplanıyorsa bekle

            if (!Agent.hasPath || Agent.remainingDistance <= 0.1f && Agent.velocity.magnitude < 0.1f)
            {
                if (!isRunning && targetList.Count == 0) return;
                StopRunning();
                Agent.enabled = false;
                _obstacle.enabled = true;
            }
        }

        private void StartRunning()
        {
            isRunning = true;
            botAnimationController.Run();
        }

        private void StopRunning()
        {
            isRunning = false;
            botAnimationController.Idle();
        }

        public void BotReset()
        {
            target = transform;
            hit.gameObject.SetActive(false);
            tower = null;
            distanceStop = 0;
            targetList.Clear();
        }
    }
}
