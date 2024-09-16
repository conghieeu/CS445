using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class StaffPoolerStats : ObjectStats
    {

        /// <summary> Load dữ liệu theo GameData </summary>
        public override void OnSetData<T>(T data)
        {
            List<StaffData> staffsData = (data as GameData)._gamePlayData.StaffsData;

            // tái tạo items data
            foreach (var staffData in staffsData)
            {
                // load data những đối tượng có sẵn
                ObjectPool staff = GetComponent<StaffPooler>().GetObjectByID(staffData.Id);
                if (staff)
                {
                    staff.GetComponent<StaffStats>().OnSetData(staffData);
                }
                else
                {
                    // tạo lại item slot
                    ObjectPool newStaff = GetComponent<StaffPooler>().GetOrCreateObjectPool(staffData.TypeID);
                    newStaff.GetComponent<StaffStats>().OnSetData(staffData);
                }
            }
        }

        protected override void LoadNewGame()
        {
            SaveData();
        }

        protected override void LoadNewData()
        {
            SaveData();
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

        public override void OnLoadData() { }
    }
}
