using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{
    public class Customer : AIBehavior
    {
        [Header("Customer")]
        [SerializeField] float _totalPay;
        [SerializeField] Item _itemFinding; // item mà khách hàng đang tìm
        [SerializeField] Transform _slotWaiting; // Hàng chờ (WaitingLine) modun của máy tính sẽ SET thứ này
        [SerializeField] Transform _outShopPoint; // Là điểm sẽ tới nếu rời shop
        [SerializeField] bool _isDoneShopping; // Không cần mua gì nữa
        [SerializeField] bool _isPlayerConfirmPay;
        [SerializeField] bool _isPickingItem; // để set animation
        [SerializeField] List<TypeID> _listItemBuy; // Cac item can lay, giới hạn là 15 item
        [SerializeField] List<Item> _itemsCard; // cac item da mua

        ReputationSystem _reputationSystem; // Thêm tham chiếu đến ReputationSystem

        public float TotalPay { get => _totalPay; }
        public bool IsDoneShopping { get => _isDoneShopping; }
        public Transform SlotWaiting { get => _slotWaiting; set => _slotWaiting = value; }
        public List<Item> ItemsCard { get => _itemsCard; }
        public bool IsPlayerConfirmPay { get => _isPlayerConfirmPay; set => _isPlayerConfirmPay = value; }

        protected override void Start()
        {
            base.Start();
            _outShopPoint = CustomerPooler.Instance.GoOutShopPoint.transform;
            _reputationSystem = ReputationSystem.Instance;
        }

        private void FixedUpdate()
        {
            SetItemNeed();
            Behavior();
            SetAnimation();
        } 

        private void OnDisable()
        {
            _itemsCard.Clear();
            _listItemBuy.Clear();
            _isDoneShopping = false;
            _isPlayerConfirmPay = false;
            _totalPay = 0;
        }

        public override void OnCreate()
        {
            base.OnCreate();
            _totalPay = 0;
        }

        /// <summary> Set Properties with Item Data </summary>
        public void SetProperties(CustomerData data)
        {
            _ID = data.Id;
            _isDoneShopping = data.IsNotNeedBuy;
            _name = data.Name;
            _totalPay = data.TotalPay;

            IsPlayerConfirmPay = data.PlayerConfirmPay;
            transform.position = data.Position;
            transform.rotation = data.Rotation;
        }

        /// <summary> Hành vi </summary>
        private void Behavior()
        {
            // đi lấy item thứ cần mua
            if (GoToItemNeed())
            {
                In($"Đi tới item muốn mua");
                return;
            }

            // chấp nhận thông số item và mua item
            if (IsAgreeItem())
            {
                In($"Lấy item");
                StartCoroutine(PickItem()); // trigger animation

                _totalPay += _itemFinding._price;
                _itemsCard.Add(_itemFinding);
                _listItemBuy.Remove(_itemFinding._typeID);
                _itemFinding._itemParent._itemSlot.RemoveItemInList(_itemFinding);
                _itemFinding.gameObject.SetActive(false);
                _itemFinding = null;

                return;
            }
            else
            {
                if (_isDoneShopping == false)
                {
                    In($"Giá quá cao");
                    _isDoneShopping = true;

                    // Cập nhật danh tiếng khi phàn nàn
                    _reputationSystem.UpdateReputation(ReputationSystem.CustomerAction.Complain);
                }
            }

            // delay cho animation pick up item
            if(_isPickingItem) return;

            // không mua được gì Out shop
            if (_totalPay == 0 && (!_itemFinding || _isDoneShopping))
            {
                In("không mua được gì Out shop");
                GoOutShop();
                return;
            }

            // không mua được nữa nhưng có item nên là thanh toán
            if (_totalPay > 0 && (!_itemFinding || _isDoneShopping))
            {
                In("Mua được vài thứ, đi thanh toán");
                _listItemBuy.Clear();

                if (!_isPlayerConfirmPay)
                {
                    GoToWating();
                }
                else
                {
                    // Cập nhật danh tiếng khi mua hàng
                    _reputationSystem.UpdateReputation(ReputationSystem.CustomerAction.Buy);

                    _mayTinh._waitingLine.CancelRegisterSlot(this);
                    GoOutShop();
                }
            }
        }

        private void GoToWating()
        {
            _mayTinh._waitingLine.RegisterSlot(this); // Đăng ký slot 
            if (_slotWaiting)
            {
                MoveToTarget(_slotWaiting);
            }
        }

        private void SetAnimation()
        {
            // is pick item
            if (_isPickingItem && _stageAnim != STATE_ANIM.Picking)
            {
                _stageAnim = STATE_ANIM.Picking;
                SetAnim();
                return;
            }

            // Idle
            if (_navMeshAgent.velocity.sqrMagnitude == 0 && _stageAnim != STATE_ANIM.Idle)
            {
                _stageAnim = STATE_ANIM.Idle;
                SetAnim();
                return;
            }

            // Walk
            if (_navMeshAgent.velocity.sqrMagnitude > 0.1f && _stageAnim != STATE_ANIM.Walk)
            {
                _stageAnim = STATE_ANIM.Walk;
                SetAnim();
                return;
            }
        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        private void SetItemNeed()
        {
            if (_listItemBuy.Count == 0 && ItemsCard.Count == 0) // đk để được set danh sách mua
            {
                if (_listItemBuy.Count >= 0)
                {
                    _listItemBuy.Clear(); // Item muốn mua không còn thì reset ds
                }

                // Tạo một số ngẫu nhiên giữa minCount và maxCount
                int countBuy = UnityEngine.Random.Range(3, 3);

                // Thêm danh sach item muon mua
                for (int i = 0; i < countBuy; i++)
                {
                    _listItemBuy.Add(GetRandomItemBuy());
                }
            }
        }

        private TypeID GetRandomItemBuy()
        {
            Debug.LogWarning($"Chỗ này chưa xong");
            return TypeID.apple_1;
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        private bool GoToItemNeed()
        {
            if (_isDoneShopping || _isPickingItem) return false;

            // set item finding
            if (_listItemBuy.Count > 0 && !_itemFinding) _itemFinding = ItemPooler.Instance.ShuffleFindItem(_listItemBuy[0]);

            Item shelf = _itemPooler.GetItemContentItem(_itemFinding); // lấy cái bàn chứa quả táo

            if (shelf && shelf._waitingPoint && !MoveToTarget(shelf._waitingPoint))
            {
                return true;
            }
            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        private void GoOutShop()
        {
            if (MoveToTarget(_outShopPoint))
            {
                // xoá tắt cả item dang giữ
                foreach (var item in ItemsCard)
                {
                    ItemPooler.Instance.RemoveObjectFromPool(item);
                }
                CustomerPooler.Instance.RemoveObjectFromPool(this);
            }
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        private bool IsAgreeItem()
        {
            return _itemFinding && _itemFinding._price <= _itemFinding._SO._priceMarketMax;
        }

        /// <summary> trigger animation picking </summary>
        private IEnumerator PickItem()
        {
            _isPickingItem = true;
            yield return new WaitForSeconds(2f);
            _isPickingItem = false;

        }
    }
}
