using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Managers;
using StateBot;
using StateBot.IBotControllerStates;
using StateBot.IBotStates;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Serialization;
using Slider = UnityEngine.UI.Slider;

namespace Controllers.EnemyController
{
    public class BotController : MonoBehaviour
    {
        #region Self Variables

        #region Public Variables
        
        public Slider Slider;
        public BotData botData;

        #endregion

        #region Serialized Variables

        [SerializeField] private BotAIController botAIController;
        [SerializeField] private BotAnimationController botAnimationController;
        [SerializeField] private BotManager _botManager;
        [SerializeField] private GameObject healtBar;
        [SerializeField] private List<BotManager> _enemyList;
        [SerializeField] private List<SkinnedMeshRenderer> _materials;
        
        #endregion

        #region Private Variables
        
        private IBotBaseState _currentBaseState;
        private IdleState _ıdleState;
        private AttackState _attackState;
        private DeadState _deadState;
        private MovetoMouseHitState _movetoMouseHitState;

        private List<(GameObject,BotController)> targetList;
        private int _health;
        private int _damage;
        private List<(int,int)> lwl;
        
        public float countdownTime = 5f; // Sayaç süresi (saniye)
        private float timer;
        private bool Enemy = false;
        private TowerManager tower;

        #endregion

        #endregion

        private void Awake()
        {

            
            _health = botData.Health;
            _damage = botData.Damage;
            
            botAIController.OnSpeed(botData.Speed);
            lwl = botData.Lwl;
        }

        private void Update()
        {
            HealtBarRotation();
        }

        public void NextLwl()
        {
            _health += (_health / (100 / lwl[_botManager.lwl].Item1));
            _damage += (_damage / (100 / lwl[_botManager.lwl].Item2));
        }

        public void SetMaterial(Material material)
        {
            foreach (var VARIABLE in _materials)
            {
                    if (VARIABLE != null)
                    {
                        // Materyalin bir kopyasını alıyoruz, böylece orijinal materyali etkilemeyiz
                        Material mat = new Material(material);
                        VARIABLE.materials[0].color = mat.color;
                        // Outline rengi varsa ilk değerini atıyoruz
                    }
            }
        }

        public void EnemyTower(TowerManager towerManager)
        {
            tower = towerManager;
            botAIController.EnemyTower(towerManager.transform);
        }

        public void AddTarget(BotManager botManager)
        {
            if (transform.CompareTag("Dead")) return;
            botAIController.AddTarget(botManager.transform);
            _enemyList.Add(botManager);
        }

        public void RemoveTarget(BotManager botManager)
        {
            _enemyList.Remove(botManager);
            botAIController.RemoveTarget(botManager.transform);
        }

        public void GetBotData(BotData Data)
        {
            botData = Data;
        }

        public void SetHealt(float healt)
        {
            Slider.value = healt / 100;
        }
        
        private void HealtBarRotation()
        {
            healtBar.transform.localEulerAngles = new Vector3(0, -transform.eulerAngles.y, 0);
        }
        
        public bool HealtDamage(int damage, string team)
        {
            _health -= damage;
            SetHealt(_health);
            if (_health < 0)
            {
                transform.tag = "Dead";
                botAnimationController.Dead();
                DOVirtual.DelayedCall(1.30f, () =>
                {
                    _botManager.SetPool();
                });
                return true;
            }

            return false;
        }

        public void BotReset()
        {
            _enemyList.Clear();
            SetHealt(100);
            NotAttack();
            _health = botData.Health;
            botAnimationController.Idle();
            botAIController.BotReset();
        }
        public void Attack()
        {
            if (transform.CompareTag("Dead")) return;
            botAnimationController.Fight();
            EnemyTrigger();
        }

        public void NotAttack()
        {
            NotEnemyTrigger();
            if (_enemyList.Count != 0)
            {
                botAnimationController.Run();
            }
        }

        #region AtackTimer

        private void EnemyTrigger()
        {
            Enemy = true;
        }
        
        private void NotEnemyTrigger()
        {
            Enemy = false;
        }
        
        private void Start()
        {
            StartTimer(countdownTime);
        }

        public void StartTimer(float duration)
        {
            timer = duration;
        }

        public void TargetDead(BotManager bot)
        {
            if (!_enemyList.Contains(bot)) return;
            StartTimer(countdownTime);
            RemoveTarget(bot);
            botAIController.RemoveTarget(bot.transform);
        }

        public void AtackTimer()
        {
            if (_enemyList.Count == 0 && Enemy == false || transform.CompareTag("Dead")) return;
            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                if (_enemyList.Count == 0) return; // 🛑 Burada da kontrol et

                Vector3 direction = (_enemyList[0].transform.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f); // Yavaşça dönme
                 _enemyList[0].OnHitDamage(_damage,tag);
                
                StartTimer(countdownTime);
            }
        }
        
        #endregion
    }
}