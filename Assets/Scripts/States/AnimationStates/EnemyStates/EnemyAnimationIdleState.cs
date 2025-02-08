using Controllers.EnemyController;
using Extentions;
using UnityEngine;

namespace States.PlayerStates
{
    public class EnemyAnimationIdleState : EnemyAnimationBaseState
    {
        public override void EnterState(BotAnimationController botState)
        {
            botState.GetAnimator().SetBool("Walking", false);
            botState.GetAnimator().SetBool("Fight", false);
            botState.GetAnimator().SetBool("Idle", true);
            botState.GetAnimator().SetBool("Run", false);
            botState.GetAnimator().SetBool("Dead", false);
        }

        public override void UpdateState(BotAnimationController playerState)
        {
            
        }
        
        public override void OnCollisionEnter(BotAnimationController playerState)
        {
            
        }
    }
}