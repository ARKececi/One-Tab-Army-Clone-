using Controllers;
using Controllers.EnemyController;
using Extentions;
using Managers;
using UnityEngine;

namespace States.PlayerStates
{
    public class EnemyAnimationFightState : EnemyAnimationBaseState
    {
        public override void EnterState(BotAnimationController botState)
        {
            botState.GetAnimator().SetBool("Walking", false);
            botState.GetAnimator().SetBool("Fight", true);
            botState.GetAnimator().SetBool("Idle", false);
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