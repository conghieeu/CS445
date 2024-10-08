
using System.Collections;
using System.Collections.Generic;
using CuaHang.Core;
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
        [SerializeField] bool _isPlayerConfirmPay;
        [SerializeField] bool _isPickingItem; // để set animation
        [SerializeField] List<TypeID> _listItemBuy; // Cac item can lay, giới hạn là 15 item
        [SerializeField] Transform m_DispawnPoint;

        CustomerPooler m_CustomerPooler;
        CustomerSpawner m_CustomerSpawner;

        PlayerCtrl m_PlayerCtrl;

        public float TotalPay { get => _totalPay; set => _totalPay = value; }
        public Transform SlotWaiting { get => _slotWaiting; set => _slotWaiting = value; }
        public bool IsPlayerConfirmPay { get => _isPlayerConfirmPay; set => _isPlayerConfirmPay = value; }
        public List<TypeID> ListItemBuy { get => _listItemBuy; set => _listItemBuy = value; }

        protected override void Awake()
        {
            base.Awake();

            m_PlayerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_CustomerPooler = FindFirstObjectByType<CustomerPooler>();
            m_CustomerSpawner = FindFirstObjectByType<CustomerSpawner>();

            _totalPay = 0;
        }

        private void OnEnable()
        {
            m_DispawnPoint = m_CustomerSpawner.GetRandomOutPoint();
            SetItemsBuy();
        }

        private void OnDisable()
        {
            ListItemBuy.Clear();
            _isPlayerConfirmPay = false;
        }

        private void FixedUpdate()
        {
            Behavior();
            SetAnimation();
        }

        /// <summary> Hành vi </summary>
        private void Behavior()
        {
            if (_isPickingItem) return;

            // set item finding
            if (ListItemBuy.Count > 0 && !_itemFinding)
            {
                _itemFinding = m_ItemPooler.GetItemByTypeID(ListItemBuy[0]);
            }

            if (_itemFinding && _itemFinding.gameObject.activeInHierarchy == false) // item muốn lấy bị xoá rồi
            {
                _itemFinding = null;
                return;
            }

            if (_itemFinding && MoveToItemFinding())
            {
                if (IsAgreeItem())
                {
                    In($"Lấy item mua");
                    StartCoroutine(PickItem()); // trigger animation 
                    _totalPay += _itemFinding.Price;
                    ListItemBuy.Remove(_itemFinding.TypeID);
                    _itemFinding.EntityParent.GetComponentInChildren<ItemSlot>().RemoveItemInList(_itemFinding);
                    _itemFinding.RemoveThis();
                    _itemFinding = null;
                }
                else
                {
                    In($"Giá quá cao");
                    ListItemBuy.Clear();
                    m_PlayerCtrl.UpdateReputation(CustomerAction.Complain);
                }
            }

            if (_itemFinding == null) // không có item muốn mua nữa
            {
                _listItemBuy.Clear();

                if (_totalPay == 0) // không mua được gì Out shop
                {
                    In("Không có gì để mua");
                    GoOutShop();
                }
                else if (_totalPay > 0) // đi thanh toán
                {
                    In("Thanh toán");
                    if (_isPlayerConfirmPay)
                    {
                        m_Computer._waitingLine.CancelRegisterSlot(this);
                        GoOutShop();
                    }
                    else
                    {
                        m_Computer._waitingLine.RegisterSlot(this); // Đăng ký slot ĐỢI
                        if (_slotWaiting)
                        {
                            MoveToTarget(_slotWaiting);
                        }
                    }
                }
            }
        }

        private void SetAnimation()
        {
            // is pick item
            if (_isPickingItem && AnimationState != STATE_ANIM.Picking)
            {
                AnimationState = STATE_ANIM.Picking;
                SetAnim();
                return;
            }

            // Idle
            if (m_NavMeshAgent.velocity.sqrMagnitude == 0 && AnimationState != STATE_ANIM.Idle)
            {
                AnimationState = STATE_ANIM.Idle;
                SetAnim();
                return;
            }

            // Walk
            if (m_NavMeshAgent.velocity.sqrMagnitude > 0.1f && AnimationState != STATE_ANIM.Walk)
            {
                AnimationState = STATE_ANIM.Walk;
                SetAnim();
                return;
            }
        }

        /// <summary> Chọn ngẫu nhiên item mà khách hàng này muốn lấy </summary>
        private void SetItemsBuy()
        {
            if (ListItemBuy.Count == 0) // đk để được set danh sách mua
            {
                if (ListItemBuy.Count >= 0)
                {
                    ListItemBuy.Clear(); // Item muốn mua không còn thì reset ds
                }

                // Tạo một số ngẫu nhiên giữa minCount và maxCount
                int countBuy = UnityEngine.Random.Range(2, 5);

                // Thêm danh sach item muon mua
                for (int i = 0; i < countBuy; i++)
                {
                    ListItemBuy.Add(GetRandomItemBuy());
                }
            }
        }

        private TypeID GetRandomItemBuy()
        {
            TypeID[] items = { TypeID.AppleA, TypeID.MilkA, TypeID.BananaA };
            int randomIndex = UnityEngine.Random.Range(0, items.Length);
            return items[randomIndex];
        }

        /// <summary> Chạy tới vị trí item cần lấy </summary>
        private bool MoveToItemFinding()
        {
            Item shelf = m_ItemPooler.GetItemContentItem(_itemFinding); // lấy cái bàn chứa quả táo

            if (shelf && shelf.WaitingPoint && MoveToTarget(shelf.WaitingPoint))
            {
                return true;
            }
            return false;
        }

        /// <summary> Ra về khách tìm điểm đến là ngoài ở shop </summary>
        private void GoOutShop()
        {
            if (MoveToTarget(m_DispawnPoint))
            {
                m_CustomerPooler.RemoveEntityFromPool(this);
            }
        }

        /// <summary> Giá quá cao thì không đồng ý mua </summary>
        private bool IsAgreeItem()
        {
            return _itemFinding && _itemFinding.Price <= _itemFinding.SO._priceMarketMax;
        }

        /// <summary> trigger animation picking </summary>
        private IEnumerator PickItem()
        {
            _isPickingItem = true;
            yield return new WaitForSeconds(3f);
            _isPickingItem = false;
        }

        #region SaveData
        public override void SetVariables<T, V>(T data)
        {
            if (data is CustomerData customerData)
            {
                base.SetVariables<T, V>(data);
                TotalPay = customerData.TotalPay;
                ListItemBuy = customerData.ListItemBuy;
                IsPlayerConfirmPay = customerData.PlayerConfirmPay;
            }
        }

        public override T GetData<T, D>()
        {
            if (ID != "")
            {
                CustomerData data = new CustomerData(GetEntityData(), TotalPay, ListItemBuy, IsPlayerConfirmPay);
                return (T)(object)data;
            }
            else
            {
                return (T)(object)null;
            } 
        }
        #endregion
    }

    // Định nghĩa enum cho các hành động của khách hàng
    public enum CustomerAction
    {
        Buy,
        Return,
        Complain,
        Praise

    }
}
