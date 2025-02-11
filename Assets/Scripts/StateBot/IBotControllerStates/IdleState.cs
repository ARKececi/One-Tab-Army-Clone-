using Controllers.EnemyController;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace StateBot.IBotStates
{
    public class IdleState : IBotBaseState
    {
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        private Animator animator;
        public IdleState(ref NavMeshAgent agent, ref NavMeshObstacle obstacle, ref Animator animator)
        {
            this.agent    = agent;
            this.obstacle = obstacle;
            this.animator = animator;
        }
        public void EnterState(BotManager bot)
        {
            NonTarget();
        }

        public void UpdateState(BotManager bot)
        {

        }

        public void OnTriggerEnterState(BotManager bot)
        {
            
        }

        public void OnTriggerExitState(BotManager bot)
        {
            
        }
        
        private void NonTarget()
        {
            agent.stoppingDistance = 0;
            obstacle.enabled = false;
            agent.enabled = true;
            Idle();
        }
        
        private void Idle()
        {
            animator.SetBool("Fight", false);
            animator.SetBool("Idle", true);
            animator.SetBool("Run", false);
            animator.SetBool("Dead", false);
        }
    }
}