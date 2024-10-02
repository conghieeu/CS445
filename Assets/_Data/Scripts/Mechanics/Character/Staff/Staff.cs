using UnityEngine;
using CuaHang.Pooler;

namespace CuaHang.AI
{
    public class Staff : AIBehavior, ISaveData
    {
        [Header("STAFF")]
        [SerializeField] Item _heldItem; // item đã nhặt và đang giữ trong người
        [SerializeField] Transform _itemHoldPos; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người  

        Item _heldItemCurrent; // trigger của animation ngăn animation được gọi liên tục từ fixed Update 

        private void FixedUpdate()
        {
            Behavior();
            PlayAnimation();
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
            Item shelf = m_ItemPooler.GetItemEmptySlot(Type.Shelf);
            if (shelf && parcelHasItem)
            {
                if (MoveToTarget(shelf.WaitingPoint.transform))
                {
                    shelf.ItemSlot.ReceiverItems(_heldItem.ItemSlot, true);
                }
                return;
            }

            // Đặt ObjectPlant vào kho
            Item storage = m_ItemPooler.GetItemEmptySlot(Type.Storage);
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
            Trash trash = m_ItemPooler.GetItemEmptySlot(Type.Trash).GetComponent<Trash>();
            if (!parcelHasItem && trash)
            {
                if (MoveToTarget(trash.transform))
                {
                    trash.ItemSlot.TryAddItemToItemSlot(_heldItem, true);
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

        private void PlayAnimation()
        {
            float velocity = m_NavMeshAgent.velocity.sqrMagnitude;

            // Idle
            if (velocity == 0 && AnimationState != STATE_ANIM.Idle || velocity == 0 && _heldItemCurrent != _heldItem)
            {
                if (_heldItem)
                {
                    AnimationState = STATE_ANIM.Idle_Carrying;
                }
                else
                {
                    AnimationState = STATE_ANIM.Idle;
                }

                _heldItemCurrent = _heldItem;
                SetAnim();
                return;
            }

            // Walk
            if (velocity > 0.1f && AnimationState != STATE_ANIM.Walk || velocity > 0.1f && _heldItemCurrent != _heldItem)
            {
                if (_heldItem)
                {
                    AnimationState = STATE_ANIM.Walk_Carrying;
                }
                else
                {
                    AnimationState = STATE_ANIM.Walk;
                }

                _heldItemCurrent = _heldItem;
                SetAnim();
                return;
            }

        }

        /// <summary> Nhân viên này tìm item cần mang vác xử lý </summary>
        private Item FindCarryItem()
        {
            foreach (var objectPool in m_ItemPooler.ListEntity)
            {
                Item item = objectPool.GetComponent<Item>();

                if (item && item.Type == Type.Parcel && !item.ThisParent && item.gameObject.activeSelf)
                {
                    return item;
                }
            }
            return null;
        }

        public override T GetData<T, D>()
        {
            StaffData data = new StaffData(GetEntityData());
            return (T)(object)(data);
        }
    }
}