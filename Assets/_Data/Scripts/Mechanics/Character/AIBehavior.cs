using System;
using UnityEngine;
using UnityEngine.AI;
using CuaHang.Pooler;

namespace CuaHang.AI
{


    public class AIBehavior : Entity
    {
        [Header("AIBehavior")]
        [SerializeField] protected SensorCast _boxSensor;
        [SerializeField] protected float m_StopDistance = 0.5f;

        protected MayTinh m_Computer;
        protected NavMeshAgent m_NavMeshAgent;
        protected GameManager m_GameManager;
        protected Animator m_Animator;
        protected ItemPooler m_ItemPooler;

        public STATE_ANIM AnimationState { get; protected set; }

        protected virtual void Awake()
        {
            m_ItemPooler = FindFirstObjectByType<ItemPooler>();
            m_GameManager = FindFirstObjectByType<GameManager>();
            m_Computer = FindFirstObjectByType<MayTinh>();

            _boxSensor = GetComponentInChildren<SensorCast>();
            m_NavMeshAgent = GetComponent<NavMeshAgent>();
            m_Animator = GetComponentInChildren<Animator>();
        }

        protected virtual void SetAnim()
        {
            m_Animator.SetInteger("State", (int)AnimationState);
        }

        /// <summary> Di chuyển đến target và trả đúng nếu đến được đích </summary>
        protected virtual bool MoveToTarget(Transform target)
        {
            m_NavMeshAgent.SetDestination(target.transform.position);

            // Kiểm tra tới được điểm target
            float distance = Vector3.Distance(transform.position, target.position);

            if (distance <= m_StopDistance)
            {
                return true;
            }

            return false;
        }
    }

    [Serializable]
    public enum STATE_ANIM
    {
        Idle = 0,
        Walk = 1,
        Picking = 2,
        Idle_Carrying = 3,
        Walk_Carrying = 4,
    }
}