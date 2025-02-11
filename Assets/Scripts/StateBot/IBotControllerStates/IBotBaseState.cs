using Controllers.EnemyController;
using Managers;

namespace StateBot
{
    public interface IBotBaseState
    {
        void EnterState(BotManager bot);
        void UpdateState(BotManager bot);
        
        void OnTriggerEnterState(BotManager bot);
        
        void OnTriggerExitState(BotManager bot);
    }
}