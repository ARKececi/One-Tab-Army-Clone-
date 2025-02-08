using Controllers;
using Controllers.EnemyController;
using Extentions;
using Managers;
using UnityEngine;

namespace States.PlayerStates
{
    public class EnemyAnimationDeadState : EnemyAnimationBaseState
    {
        public override void EnterState(BotAnimationController botState)
        {
            botState.GetAnimator().SetBool("Walking", false);
            botState.GetAnimator().SetBool("Fight", false);
            botState.GetAnimator().SetBool("Idle", false);
            botState.GetAnimator().SetBool("Run", false);
            botState.GetAnimator().SetBool("Dead", true);
        }

        public override void UpdateState(BotAnimationController playerState)
        {
            
        }
        
        public override void OnCollisionEnter(BotAnimationController playerState)
        {
            
        }
    }
}