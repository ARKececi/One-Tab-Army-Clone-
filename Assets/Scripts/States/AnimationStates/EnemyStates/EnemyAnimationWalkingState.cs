using Controllers;
using Controllers.EnemyController;
using Extentions;
using Managers;
using UnityEngine;

namespace States.PlayerStates
{
    public class EnemyAnimationWalkingState : EnemyAnimationBaseState
    {
        public override void EnterState(BotAnimationController botState)
        {
            botState.GetAnimator().SetBool("Walking", true);
            botState.GetAnimator().SetBool("Fight", false);
            botState.GetAnimator().SetBool("Idle", false);
            botState.GetAnimator().SetBool("Run", false);
            botState.GetAnimator().SetBool("Dead", false);
        }

        public override void UpdateState(BotAnimationController botState)
        {
            
        }
        
        public override void OnCollisionEnter(BotAnimationController botState)
        {
            
        }
    }
}