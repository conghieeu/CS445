using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class StaffPooler : EntityPooler
    {
        public override void SetVariables<T, V>(T data)
        {
            if (data is GamePlayData gamePlayData)
            {
                base.SetVariables<List<StaffData>, StaffData>(gamePlayData.StaffsData);
            }
        }

        public override void SaveData()
        {
            DataManager.Instance.GameData._gamePlayData.StaffsData = GetData<List<StaffData>, StaffData>();
        }
    }
}
