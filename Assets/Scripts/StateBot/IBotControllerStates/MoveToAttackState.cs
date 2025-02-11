using Controllers.EnemyController;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace StateBot.IBotControllerStates
{
    public class MoveToAttackState : IBotBaseState
    {
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        private Collider collider;
        private Vector3 target;
        
        public MoveToAttackState( ref NavMeshAgent agent, ref NavMeshObstacle obstacle, ref Collider collider)
        {
            this.agent = agent;
            this.obstacle = obstacle;
            this.collider = collider;
        }
        public void EnterState(BotManager bot)
        {
            
        }

        public void UpdateState(BotManager bot)
        {
            
        }

        public void OnTriggerEnterState(BotManager bot)
        {
            if (!collider.CompareTag(bot.tag) && collider.TryGetComponent<BotManager>(out var botManager))
            {
                agent.SetDestination(botManager.transform.position);
                bot.target = botManager;
                Debug.Log(target);
            }
        }

        public void OnTriggerExitState(BotManager bot)
        {

        }
    }
}