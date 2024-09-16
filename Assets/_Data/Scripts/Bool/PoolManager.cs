using UnityEngine;

namespace CuaHang.Pooler
{
    public class PoolManager : Singleton<PoolManager>
    {
        public ObjectPool FindObjectPoolByID(string id)
        {
            ObjectPool[] objectPools = FindObjectsOfType<ObjectPool>();

            foreach (var pool in objectPools)
            {
                if (pool.ID == id)
                {
                    return pool;
                }
            }
            return null; // Trả về null nếu không tìm thấy
        }
  
    }
}

