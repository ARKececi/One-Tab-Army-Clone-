using Controllers.EnemyController;
using Managers;
using UnityEngine;
using UnityEngine.AI;

namespace StateBot.IBotStates
{
    public class AttackState : IBotBaseState
    {
        private BotManager       targetManager;
        private float            countdownTime;
        private float            timer;
        private Collider         attackcol;

        public AttackState(ref float countdownTime, ref Collider attackcol)
        {
            this.countdownTime = countdownTime;
            this.attackcol     = attackcol;
        }

        public void EnterState(BotManager bot)
        {
            
        }

        public void UpdateState(BotManager bot)
        {
            AtackTimer(bot);
        }

        public void OnTriggerEnterState(BotManager bot)
        {
            
        }

        public void OnTriggerExitState(BotManager bot)
        {
            
        }
        
        public void StartTimer(float duration)
        {
            timer = duration;
        }
        
        public void AtackTimer(BotManager me)
        {
            if (me.target == null || me.CompareTag("Dead")) return;
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                Vector3 direction = (targetManager.transform.position - me.transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                me.transform.rotation = Quaternion.Slerp(me.transform.rotation, lookRotation, Time.deltaTime * 5f); // Yavaşça dönme
                me.target.OnHitDamage(me.Damage,me.tag);
                if (me.target.CompareTag("Dead"))
                    me.NewTarget();
                StartTimer(countdownTime);
            }
        }
    }
}