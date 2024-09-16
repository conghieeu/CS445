using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.AI
{
    public class StaffStats : ObjectStats
    {
        [Header("ItemStats")]
        [SerializeField] StaffData _staffData;

        public virtual StaffData GetData()
        {
            Staff staff = GetComponent<Staff>();

            _staffData = new StaffData(
                staff.ID,
                staff.TypeID,
                staff.Name,
                staff.transform.position);

            return _staffData;
        }

        public override void OnSetData<T>(T data)
        {
            _staffData = data as StaffData;

            if (_staffData == null) return;

            Staff staff = GetComponent<Staff>();
            staff.SetProperties(_staffData);
        }

        protected override void SaveData() { }

        protected override void LoadNewGame() { }

        protected override void LoadNewData() { }
    }

}