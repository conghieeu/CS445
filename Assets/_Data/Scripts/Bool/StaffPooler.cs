using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class StaffPooler : EntityPooler, ISaveData
    {
        public static StaffPooler Instance;

        protected override void Awake()
        {
            base.Awake();

            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = this;
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.StaffsData = GetData<List<StaffData>, StaffData>();
        }
    }
}
