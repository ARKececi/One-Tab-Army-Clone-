using Controllers.EnemyController;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace StateBot.IBotControllerStates
{
    public class MovetoMouseHitState : IBotBaseState
    {
        private NavMeshAgent agent;
        private NavMeshObstacle obstacle;
        private Animator animator;
        public MovetoMouseHitState(ref NavMeshAgent agent, ref NavMeshObstacle obstacle, ref Animator animator)
        {
            this.agent    = agent;
            this.obstacle = obstacle;
            this.animator = animator;
        }
        public void EnterState(BotManager bot)
        {
            
            HitTarget(bot);
            
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
        
        public void HitTarget(BotManager bot)
        {
            agent.stoppingDistance = 0;
            obstacle.enabled = false;
            agent.enabled = true;
            agent.SetDestination(bot.hit);
            Run();
        }
        
        public void Run()
        {
            animator.SetBool("Fight", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Run", true);
            animator.SetBool("Dead", false);
        }
    }
}