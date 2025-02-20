
using UnityEngine;

namespace CuaHang
{
    public class MayTinh : Item
    {
        [Header("MayTinh")]
        public ItemSO _objectPlantSO;
        public Transform _spawnTrans;
        public WaitingLine _waitingLine; 
 
        protected override void Awake()
        {
            base.Awake();
            _waitingLine = GetComponentInChildren<WaitingLine>();
        }

        /// <summary> Đặt lại toạ độ trục Y = 0 để nó khớp với sàn </summary>
        public override void DropItem(Transform location)
        {
            base.DropItem(location);

            // Set Y
            for (int i = 0; i < _waitingLine._waitingSlots.Count; i++)
            {
                Vector3 iPos = _waitingLine._waitingSlots[i]._slot.transform.position;

                iPos.y = 0;

                _waitingLine._waitingSlots[i]._slot.transform.position = iPos;
            }
        }
 

    }
}