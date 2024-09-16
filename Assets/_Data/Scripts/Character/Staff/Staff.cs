using UnityEngine;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class Staff : AIBehavior
    {
        [Header("STAFF")]
        public Item _heldItem; // item đã nhặt và đang giữ trong người
        
        Item _heldItemCurrent; // trigger của animation ngăn animation được gọi liên tục từ fixed Update

        [SerializeField] Transform _itemHoldPos; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người 

        protected override void Start()
        {
            base.Start();
            _itemHoldPos = transform.Find("ITEM_HOLD_POS");
        }

        private void FixedUpdate()
        {
            Behavior();
            Animation();
        }

        /// <summary> Được gọi stats </summary>
        public void SetProperties(StaffData data)
        {
            ID = data.Id;
            _name = data.Name;
            transform.position = data.Position;
        }

        /// <summary>  Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm </summary>
        private void Behavior()
        {
            // Find the parcel
            Item itemCarry = null;
            if (!_heldItem) itemCarry = FindCarryItem();

            // Nhặt parcel
            if (itemCarry && MoveToTarget(itemCarry.transform))
            {
                SetHeldItem(itemCarry);
                return;
            }

            // Parcel có item không
            bool parcelHasItem = false;
            if (_heldItem)
            {
                parcelHasItem = _heldItem.ItemSlot.IsAnyItem();
            }

            if (_heldItem == null) return;

            // Đưa item lênh kệ
            Item shelf = ItemPooler.Instance.GetItemEmptySlot(TypeID.shelf_1);
            if (shelf && parcelHasItem)
            {
                if (MoveToTarget(shelf.WaitingPoint.transform))
                {
                    shelf.ItemSlot.ReceiverItems(_heldItem.ItemSlot, true);
                }
                return;
            }

            // Đặt ObjectPlant vào kho
            Item storage = ItemPooler.Instance.GetItemEmptySlot(TypeID.storage_1);
            if (storage && parcelHasItem)
            {
                if (MoveToTarget(storage.transform))
                {
                    storage.ItemSlot.TryAddItemToItemSlot(_heldItem, true);
                    _heldItem.IsCanDrag = true;
                    _heldItem = null;
                }
                return;
            }

            // Đặt ObjectPlant vào thùng rác 
            Trash trash = ItemPooler.Instance.GetItemEmptySlot(TypeID.trash_1).GetComponent<Trash>();
            if (!parcelHasItem && trash)
            {
                if (MoveToTarget(trash.transform))
                {
                    trash.ItemSlot.TryAddItemToItemSlot(_heldItem, true);
                    trash.AddItemToTrash(_heldItem);
                    _heldItem.IsCanDrag = true;
                    _heldItem = null;
                }
                return;
            }
        }

        /// <summary> nhặt item carry lênh </summary>
        public void SetHeldItem(Item itemCarry)
        {
            itemCarry.SetParent(_itemHoldPos, this, false);
            itemCarry.IsCanDrag = false;
            _heldItem = itemCarry;
        }

        private void Animation()
        {
            float velocity = _navMeshAgent.velocity.sqrMagnitude;

            // Idle
            if (velocity == 0 && _stageAnim != STATE_ANIM.Idle || velocity == 0 && _heldItemCurrent != _heldItem)
            {
                if (_heldItem) _stageAnim = STATE_ANIM.Idle_Carrying;
                else _stageAnim = STATE_ANIM.Idle;
                _heldItemCurrent = _heldItem;
                SetAnim();
                return;
            }

            // Walk
            if (velocity > 0.1f && _stageAnim != STATE_ANIM.Walk || velocity > 0.1f && _heldItemCurrent != _heldItem)
            {
                if (_heldItem) _stageAnim = STATE_ANIM.Walk_Carrying;
                else _stageAnim = STATE_ANIM.Walk;
                _heldItemCurrent = _heldItem;
                SetAnim();
                return;
            }

        }

        /// <summary> Nhân viên này tìm item cần mang vác xử lý </summary>
        private Item FindCarryItem()
        {
            foreach (var objectPool in ItemPooler.Instance._ObjectPools)
            {
                Item item = objectPool.GetComponent<Item>();

                if (item && item.TypeID == TypeID.parcel_1 && !item.ThisParent && item.gameObject.activeSelf)
                {
                    return item;
                }
            }
            return null;
        }
    }
}