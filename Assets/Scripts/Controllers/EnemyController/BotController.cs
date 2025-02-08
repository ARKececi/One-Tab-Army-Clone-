using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using DG.Tweening;
using Enums;
using Managers;
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

        public int lwl;
        public string teamTag;
        public Slider Slider;
        public EnemyData EnemyData;

        #endregion

        #region Serialized Variables

        [FormerlySerializedAs("enemyAIController")] [SerializeField] private BotAIController botAIController;
        [FormerlySerializedAs("enemyAnimationController")] [SerializeField] private BotAnimationController botAnimationController;
        [FormerlySerializedAs("enemyAtackController")] [SerializeField] private BotAtackController botAtackController;
        
        [SerializeField] private GameObject moneyBag;
        [SerializeField] private GameObject enemyPhysics;
        [SerializeField] private GameObject healtBar;
        [SerializeField] private EnemyEnum enemyEnum;
        [SerializeField] private List<BotManager> _enemyList;
        
        #endregion

        #region Private Variables

        private List<(GameObject,BotController)> targetList;
        private int _healt;
        private int damage;
        
        public float countdownTime = 5f; // Sayaç süresi (saniye)
        private float timer;
        private bool Enemy = false;
        private TowerManager tower;

        #endregion

        #endregion

        private void Awake()
        {
            EnemyData = GetEnemyData();
            _healt = EnemyData.Healt;
            botAIController.OnSpeed(EnemyData.Speed);
            damage = EnemyData.Damage;
        }

        private void Update()
        {
            HealtBarRotation();
        }

        public void EnemyTower(TowerManager towerManager)
        {
            tower = towerManager;
            botAIController.EnemyTower(towerManager.transform);
        }

        public void AddTarget(BotManager botManager)
        {
            botAIController.AddTarget(botManager.transform);
            _enemyList.Add(botManager);
        }

        public void RemoveTarget(BotManager botManager)
        {
            _enemyList.Remove(botManager);
            botAIController.RemoveTarget(botManager.transform);
        }

        private EnemyData GetEnemyData()
        {
            return Resources.Load<CD_Enemy>("Data/CD_Enemy").EnemyDatas[enemyEnum];
        }

        public void SetHealt(float healt)
        {
            Slider.value = healt / 100;
        }
        
        private void HealtBarRotation()
        {
            healtBar.transform.localEulerAngles = new Vector3(0, -transform.eulerAngles.y, 0);
        }
        
        public void HealtDamage(int damage)
        {
            _healt -= damage;
            SetHealt(_healt);
            if (_healt < 0)
            {
                enemyPhysics.SetActive(false);
                transform.tag = "Dead";
                botAIController.BotReset();
                botAnimationController.Dead();
                
                ExpThrow();
                DOVirtual.DelayedCall(1.30f, () =>
                {
                    SetHealt(100);
                });
                _healt = EnemyData.Healt;
            }
        }

        public void BotReset()
        {
            _healt = EnemyData.Healt;
            botAnimationController.Idle();
            botAIController.BotReset();
            transform.tag = "Untagged";
        }

        private void ExpThrow()
        {

        }

        #region AtackTimer

        private void EnemyTrigger()
        {
            Enemy = true;
        }

        private void Start()
        {
            StartTimer(countdownTime);
        }

        public void StartTimer(float duration)
        {
            timer = duration;
            Enemy = false;
        }

        public void AtackTimer()
        {
            if (_enemyList.Count == 0) return;

            timer -= Time.deltaTime;

            if (timer <= 0)
            {
                _enemyList[0].OnHitDamage(damage);
                StartTimer(countdownTime);
            }
        }

        #endregion
    }
}