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

        /// <summary> Được Pooler gọi </summary>
        public override void OnSetData<T>(T data)
        {
            if (data is StaffData)
            {
                _staffData = data as StaffData;
            }
        }

        protected override void SaveData() { }

        public override void OnLoadData()
        {
            if (GetGameData()._gamePlayData.IsInitialized)
            { 
                Staff staff = GetComponent<Staff>();
                staff.SetProperties(_staffData);
            }
        }
    }

}