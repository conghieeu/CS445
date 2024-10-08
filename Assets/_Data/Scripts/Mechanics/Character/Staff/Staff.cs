using UnityEngine;

namespace CuaHang.AI
{
    public class Staff : AIBehavior, ISaveData
    {
        [Header("STAFF")]
        [SerializeField] Item itemHolding; // item đã nhặt và đang giữ trong người
        [SerializeField] Transform _itemHoldPos; // là vị trí mà nhân viên này đang giữ ObjectPlant trong người  

        Item _heldItemCurrent; // trigger của animation ngăn animation được gọi liên tục từ fixed Update 

        private void FixedUpdate()
        {
            Behavior();
            PlayAnimation();
        }

        public override void PickUpEntity(Entity entity)
        {
            base.PickUpEntity(entity);
            PickupItem(entity.GetComponent<Item>());
        }

        /// <summary>  Khi đáp ứng sự kiện hãy gọi vào đây nên nó đưa phán đoán hành vi tiếp theo nhân viên cần làm </summary>
        private void Behavior()
        {
            // Find the parcel
            Item itemCarry = null;
            if (!itemHolding) itemCarry = FindCarryItem();

            // Nhặt parcel
            if (itemCarry && MoveToTarget(itemCarry.transform))
            {
                PickupItem(itemCarry);
                return;
            }

            // Parcel có item không
            bool isHasItemInItemPickUp = false;
            if (itemHolding)
            {
                isHasItemInItemPickUp = itemHolding.ItemSlot.IsAnyItem();
            }

            if (itemHolding == null) return;

            // Đưa item lênh kệ
            Item shelf = m_ItemPooler.GetItemEmptySlot(Type.Shelf);
            if (shelf && isHasItemInItemPickUp)
            {
                if (MoveToTarget(shelf.WaitingPoint.transform))
                {
                    shelf.ItemSlot.ReceiverItems(itemHolding.ItemSlot, true);
                }
                return;
            }

            // Đặt ObjectPlant vào kho
            Item storage = m_ItemPooler.GetItemEmptySlot(Type.Storage);
            if (storage && isHasItemInItemPickUp)
            {
                if (MoveToTarget(storage.transform))
                {
                    storage.ItemSlot.TryAddItemToItemSlot(itemHolding);
                    itemHolding.IsCanDrag = true;
                    itemHolding = null;
                }
                return;
            }

            // Đặt ObjectPlant vào thùng rác 
            Item trash = m_ItemPooler.GetItemEmptySlot(Type.Trash);
            if (trash && !isHasItemInItemPickUp)
            {
                if (MoveToTarget(trash.transform))
                {
                    trash.PickUpEntity(itemHolding);
                    itemHolding = null;
                }
                return;
            }
        }

        /// <summary> nhặt item carry lênh </summary>
        public void PickupItem(Item itemCarry)
        {
            itemCarry.SetParent(_itemHoldPos, this, false);
            itemCarry.IsCanDrag = false;
            itemHolding = itemCarry;
        }

        private void PlayAnimation()
        {
            float velocity = m_NavMeshAgent.velocity.sqrMagnitude;

            // Idle
            if (velocity == 0 && AnimationState != STATE_ANIM.Idle || velocity == 0 && _heldItemCurrent != itemHolding)
            {
                if (itemHolding)
                {
                    AnimationState = STATE_ANIM.Idle_Carrying;
                }
                else
                {
                    AnimationState = STATE_ANIM.Idle;
                }

                _heldItemCurrent = itemHolding;
                SetAnim();
                return;
            }

            // Walk
            if (velocity > 0.1f && AnimationState != STATE_ANIM.Walk || velocity > 0.1f && _heldItemCurrent != itemHolding)
            {
                if (itemHolding)
                {
                    AnimationState = STATE_ANIM.Walk_Carrying;
                }
                else
                {
                    AnimationState = STATE_ANIM.Walk;
                }

                _heldItemCurrent = itemHolding;
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
            if (ID != "")
            {
                StaffData data = new StaffData(GetEntityData());
                return (T)(object)data;
            }
            else
            {
                return (T)(object)null;
            }
        }
    }
}