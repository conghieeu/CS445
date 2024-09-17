using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class StaffPoolerStats : ObjectStats
    {
        [SerializeField] List<StaffData> _staffsData;

        /// <summary> Load dữ liệu theo GameData </summary>
        public override void OnSetData<T>(T data)
        {
            if(!(data is GamePlayData)) return;

            _staffsData = (data as GamePlayData).StaffsData;

            // tái tạo items data
            foreach (var staffData in _staffsData)
            {
                ObjectPool staff = GetComponent<StaffPooler>().GetObjectByID(staffData.Id);
                if (staff) // load data những đối tượng có sẵn
                {
                    staff.GetComponent<StaffStats>().OnSetData(staffData);
                }
                else // tạo lại item slot
                {
                    ObjectPool newStaff = GetComponent<StaffPooler>().GetOrCreateObjectPool(staffData.TypeID);
                    newStaff.GetComponent<StaffStats>().OnSetData(staffData);
                }
            }
        }

        protected override void SaveData()
        {
            List<StaffData> staffsData = GetData();
            GetGameData()._gamePlayData.StaffsData = staffsData;
        }

        private List<StaffData> GetData()
        {
            List<StaffData> staffsData = new List<StaffData>();

            foreach (var objP in GetComponent<StaffPooler>()._ObjectPools)
            {
                if (objP && objP.ID != "" && objP.gameObject.activeInHierarchy)
                {
                    staffsData.Add(objP.GetComponent<StaffStats>().GetData());
                }
            }

            return staffsData;
        }

        public override void OnLoadData()
        {

        }
    }
}
