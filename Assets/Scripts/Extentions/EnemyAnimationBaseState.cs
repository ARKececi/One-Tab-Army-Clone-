using Controllers.EnemyController;

namespace Extentions
{
    public abstract class EnemyAnimationBaseState
    {
        public abstract void EnterState(BotAnimationController botState);
        public abstract void UpdateState(BotAnimationController playerState);
        public abstract void OnCollisionEnter(BotAnimationController playerState);
    }
}