using CuaHang.AI;
using UnityEngine;
using UnityEngine.InputSystem;

namespace CuaHang.Player
{
    public class PlayerMovement : GameBehavior
    {

        [SerializeField] float _moveSpeed;
        [SerializeField] Camera mainCamera;
        [SerializeField] STATE_ANIM _stageAnim;
        [SerializeField] bool _triggerDragging; // trigger player đang drag item
        [SerializeField] Vector3 _moveDir;

        Rigidbody _rb;
        Animator _animator;
        ModuleDragItem m_ModuleDragItem;
        PlayerCtrl m_PlayerCtrl;
        InputImprove m_InputImprove;

        private void Awake()
        {
            m_InputImprove = FindFirstObjectByType<InputImprove>();
            m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_ModuleDragItem = FindFirstObjectByType<ModuleDragItem>();

            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
            //_rb.angularDamping = 0.0f;
            mainCamera = Camera.main;
        }

        private void FixedUpdate()
        {
            SetAnimator();
            Movement(); 
        }

        private void Movement()
        {
            // Input
            Vector2 moveD = m_InputImprove.GetInputMove();

            // camera dir
            Vector3 camForward = mainCamera.transform.forward;
            Vector3 camRight = mainCamera.transform.right;

            camForward.y = 0;
            camRight.y = 0;

            // creating relate cam direction
            Vector3 forwardRelative = moveD.y * camForward;
            Vector3 rightRelative = moveD.x * camRight;

            _moveDir = (forwardRelative + rightRelative).normalized;

            // movement 
            //Vector3 velocity = new Vector3(_moveDir.x, _rb.linearVelocity.y, _moveDir.z) * _moveSpeed;

            //_rb.linearVelocity = velocity;

            // Trường hợp đang kéo thả Item nào đó
            //if (_rb.linearVelocity.magnitude > 0 && !m_ModuleDragItem.IsDragging)
            //{
            //    velocity.y = 0;
            //    transform.forward = velocity;
            //}
        }

        private void SetAnimator()
        {
            bool IsDragItem = m_PlayerCtrl.IsDragItem;
            
            // Idle
            if (_moveDir == Vector3.zero && (_stageAnim != STATE_ANIM.Idle || _triggerDragging != IsDragItem))
            {
                if (IsDragItem) _stageAnim = STATE_ANIM.Idle_Carrying;
                else _stageAnim = STATE_ANIM.Idle;
                _triggerDragging = IsDragItem;
                SetAnim();
                return;
            }

            // Walk
            if (_moveDir != Vector3.zero && _stageAnim != STATE_ANIM.Walk || _triggerDragging != IsDragItem)
            {
                if (IsDragItem) _stageAnim = STATE_ANIM.Walk_Carrying;
                else _stageAnim = STATE_ANIM.Walk;
                _triggerDragging = IsDragItem;
                SetAnim();
                return;
            }
        }

        private void SetAnim() => _animator.SetInteger("State", (int)_stageAnim);
    }
}
