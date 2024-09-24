using System;
using System.Collections;
using System.Collections.Generic;
using CuaHang;
using CuaHang.AI;
using UnityEngine;

namespace CuaHang.Player
{
    public class PlayerMovement : GameBehavior
    {
        [SerializeField] float _moveSpeed;
        [SerializeField] Transform _cam;
        [SerializeField] STATE_ANIM _stageAnim;
        [SerializeField] bool _triggerDragging; // trigger player đang drag item
        [SerializeField] Vector3 _moveDir;

        Rigidbody _rb;
        Animator _animator;
        InputImprove _input;
        ModuleDragItem _itemDrag;

        private void Awake()
        {
            _animator = GetComponentInChildren<Animator>();
            _rb = GetComponent<Rigidbody>();
            _rb.angularDamping = 0.0f;
        }

        private void Start()
        {
            _input = InputImprove.Instance;
            _cam = Camera.main.transform;
            _itemDrag = RaycastCursor.Instance.ItemDrag;
        }

        private void FixedUpdate()
        {
            SetAnimator();
            Movement();
        }

        private void Movement()
        {
            // Input
            float horInput = _input.MovementInput().x;
            float verInput = _input.MovementInput().y;

            // camera dir
            Vector3 camForward = _cam.forward;
            Vector3 camRight = _cam.right;

            camForward.y = 0;
            camRight.y = 0;

            // creating relate cam direction
            Vector3 forwardRelative = verInput * camForward;
            Vector3 rightRelative = horInput * camRight;

            _moveDir = (forwardRelative + rightRelative).normalized;

            // movement 
            Vector3 velocity = new Vector3(_moveDir.x, _rb.linearVelocity.y, _moveDir.z) * _moveSpeed;

            _rb.linearVelocity = velocity;

            // Trường hợp đang kéo thả Item nào đó
            if (_rb.linearVelocity.magnitude > 0 && !_itemDrag._isDragging)
            {
                velocity.y = 0;
                transform.forward = velocity;
            }
        }

        private void SetAnimator()
        {
            bool _isDragItem = _itemDrag.gameObject.activeInHierarchy;

            // Idle
            if (_moveDir == Vector3.zero && (_stageAnim != STATE_ANIM.Idle || _triggerDragging != _isDragItem))
            {
                if (_isDragItem) _stageAnim = STATE_ANIM.Idle_Carrying;
                else _stageAnim = STATE_ANIM.Idle;
                _triggerDragging = _isDragItem;
                SetAnim();
                return;
            }

            // Walk
            if (_moveDir != Vector3.zero && _stageAnim != STATE_ANIM.Walk || _triggerDragging != _isDragItem)
            {
                if (_isDragItem) _stageAnim = STATE_ANIM.Walk_Carrying;
                else _stageAnim = STATE_ANIM.Walk;
                _triggerDragging = _isDragItem;
                SetAnim();
                return;
            }
        }

        private void SetAnim() => _animator.SetInteger("State", (int)_stageAnim);
    }
}
