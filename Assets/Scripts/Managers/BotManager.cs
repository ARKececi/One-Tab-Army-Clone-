using System;
using System.Collections.Generic;
using Data.UnityObject;
using Data.ValueObject;
using Enums;
using StateBot;
using StateBot.IBotControllerStates;
using StateBot.IBotStates;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

namespace Managers
{
    public class BotManager : MonoBehaviour
    {
        #region Self Variables

        #region MyRegion

        public int lwl;
        public TowerManager TowerManager;
        public List<(int,int)> BotBufflwl;

        #endregion

        #region Serialized Variables
        
        [SerializeField] private BotType botType;

        #endregion

        #region State Variables

        private IBotBaseState       currentBaseState;
        private IdleState           ıdleState;
        private AttackState         attackState;
        private DeadState           deadState;
        private MovetoMouseHitState movetoMouseHitState;
        private MoveToAttackState   moveToAttackState;

        #endregion
        
        #region AI Variables

        #region Public Variables
         public NavMeshAgent Agent;
         public NavMeshObstacle _obstacle;
         public BotManager target; // hedef manager;

        #region Private Variables
        [SerializeField] private List<Transform> targetList = new List<Transform>();
        private float distanceStop;
        private int speed;
        private bool isRunning;
        
        private Transform tower;
        public Vector3 hit;
        #endregion
        
        #endregion
        
        #region Animation Variables

        #region Serialized Variables

        [SerializeField] private Animator animator;

        #endregion

        #endregion

        #region Controller

        #region Public Variables
        
        public Slider Slider;
        public BotData botData;
        public int Health;
        public int Damage;

        #endregion

        #region Serialized Variables
        
        [SerializeField] private GameObject healtBar;
        [SerializeField] private List<SkinnedMeshRenderer> _materials;
        [SerializeField] private Collider _fieldCol;
        [SerializeField] private Collider _attackCol;
        
        #endregion

        #region Private Variables
        

        private float countdownTime = 5f;
        private int enemyDamage;

        #endregion

        #endregion

        #endregion
        
        #endregion
        
        #region Event Subscription

        private void OnEnable()
        {
            SubscribeEvents();
        }

        private void SubscribeEvents()
        {

        }

        private void UnsubscribeEvents()
        {

        }

        private void OnDisable()
        {
            UnsubscribeEvents();
        }
        #endregion

        private void Awake()
        {
            GetBotData();
            lwl = 1;

            Transform me = transform;
            BotManager meManager = this;
            ıdleState           = new IdleState(ref Agent,ref _obstacle,ref animator);
            attackState         = new AttackState(ref countdownTime, ref _attackCol);  
            deadState           = new DeadState(ref Slider,ref me, ref animator, ref enemyDamage);
            movetoMouseHitState = new MovetoMouseHitState(ref Agent, ref _obstacle,ref animator);
            moveToAttackState   = new MoveToAttackState(ref Agent, ref _obstacle, ref _fieldCol);
            currentBaseState = ıdleState;

        }

        private void Update()
        {
            if (Agent.pathPending) return; // Hedef hesaplanıyorsa bekle
                if (!Agent.hasPath || Agent.remainingDistance <= 0.1f && Agent.velocity.magnitude < 0.1f)
                    if (target == null)
                    {
                        ıdleState.EnterState(this);
                    }
                    else
                    {
                        attackState.UpdateState(this);
                    }
        }

        public void NewTarget()
        {
            attackState.OnTriggerEnterState(this);
        }

        public void OnNextLevel()
        {
            lwl++;
            // botController.NextLwl();
        }

        public void OnSetMaterial(Material material)
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

        public void OnHitTarget(Vector3 MouseHit)
        {
            if (target != null) return;
            hit = MouseHit;
            movetoMouseHitState.EnterState(this);
        }

        public void OnHitDamage(int damge, string team)
        {
            deadState.EnterState(this);
        }

        public void SetBotTag(string tag)
        {
            transform.tag = tag;
        }

        public void SetPool()
        {
            TowerManager.AddBotToPool(botType,this);
        }
        
        private void GetBotData()
        {
            botData = Resources.Load<CD_Bot>("Data/CD_Enemy").EnemyDatas[botType]; 
        }

        #region Controller Fonks
        

        public void BotReset()
        {
            Slider.value = botData.Health;
            Health = botData.Health;
            ıdleState.EnterState(this);
            target = null;
            tower = null;
            distanceStop = 1f;
        }

        private void OnTriggerEnter(Collider other)
        {
            
        }

        private void OnTriggerExit(Collider other)
        {
            
        }

        #endregion
        
    }
}