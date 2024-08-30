using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using Core;
using UnityEngine;

namespace CuaHang
{
    public class StaffPoolerStats : ObjectStats
    {
        [Header("STAFF POOLER STATS")]
        [SerializeField] StaffPooler _staffPooler;

        protected override void Start()
        {
            base.Start();
            _staffPooler = GetComponent<StaffPooler>();
        }

        /// <summary> Load dữ liệu theo GameData </summary>
        public override void LoadData<T>(T data)
        {
            List<StaffData> staffsData = (data as GameData)._staffsData;

            // tái tạo items data
            foreach (var staffData in staffsData)
            {
                ObjectPool staff = GetComponent<StaffPooler>().GetObjectID(staffData._id);

                // load data những đối tượng đã tồn tại
                if (staff)
                {
                    staff.GetComponent<StaffStats>().LoadData(staffData);
                }
                else
                {
                    // tạo
                    ObjectPool newStaff = GetComponent<StaffPooler>().GetObjectPool(staffData._typeID);
                    newStaff.GetComponent<StaffStats>().LoadData(staffData);
                }
            }
        }

        protected override void SaveData()
        {
            List<StaffData> staffsData = new List<StaffData>();

            foreach (var pool in _staffPooler._ObjectPools)
            {
                if (pool && pool._ID != "" && pool.gameObject.activeInHierarchy)
                {
                    staffsData.Add(pool.GetComponent<StaffStats>().GetData());
                }
            }

            GetGameData()._staffsData = staffsData;
        }


    }
}
