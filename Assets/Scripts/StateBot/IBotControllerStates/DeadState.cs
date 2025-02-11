using Controllers.EnemyController;
using DG.Tweening;
using Managers;
using UnityEngine;
using UnityEngine.UI;

namespace StateBot.IBotStates
{
    public class DeadState : IBotBaseState
    {
        private Slider     slider;
        private Transform  me;
        private Animator   animator;
        private int        enemyDamage;
        public DeadState(ref Slider slider, ref Transform me,
            ref Animator animator, ref int enemyDamage)
        {
            this.slider    = slider;
            this.me        = me;
            this.animator  = animator;
            this.enemyDamage = enemyDamage;
        }
        public void EnterState(BotManager bot)
        {
            HealtDamage(bot);
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
        
        public void HealtDamage(BotManager bot)
        {
            bot.Health -= enemyDamage;
            SetHealt(bot.Health);
            if (bot.Health < 0)
            {
                me.tag = "Dead";
                Dead();
                DOVirtual.DelayedCall(1.30f, () =>
                {
                    bot.SetPool();
                });
            }
        }
        
        public void Dead()
        {
            animator.SetBool("Fight", false);
            animator.SetBool("Idle", false);
            animator.SetBool("Run", false);
            animator.SetBool("Dead", true);
        }
        
        public void SetHealt(float healt)
        {
            slider.value = healt / 100;
        }
    }
}