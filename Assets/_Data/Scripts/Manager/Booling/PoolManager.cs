using System.Collections.Generic;
using UnityEngine;

namespace CuaHang.Pooler
{
    public class PoolManager : GameBehavior
    {
        public List<Entity> Entities;

        List<EntityPooler> EntityPoolers;

        private void Awake()
        {
            RegisterEntityPoolerEvents();
        }

        // Phương thức để đăng ký sự kiện từ tất cả các EntityPooler
        public void RegisterEntityPoolerEvents()
        {
            if (EntityPoolers == null)
            {
                EntityPoolers = new List<EntityPooler>();
            }
            else
            {
                EntityPoolers.Clear();
            }

            // Tìm tất cả các đối tượng EntityPooler trong game
            EntityPooler[] allEntityPoolers = FindObjectsByType<EntityPooler>(FindObjectsSortMode.None);
            EntityPoolers.AddRange(allEntityPoolers);

            // Đăng ký sự kiện cho mỗi EntityPooler
            foreach (EntityPooler pooler in EntityPoolers)
            {
                pooler.OnPoolChange += CollectAllEntities;
            }
        }

        // Phương thức để lấy tất cả các đối tượng Entity trong game
        public void CollectAllEntities()
        {
            // Khởi tạo danh sách nếu chưa được khởi tạo
            if (Entities == null)
            {
                Entities = new List<Entity>();
            }
            else
            {
                // Xóa danh sách hiện tại để đảm bảo không có đối tượng trùng lặp
                Entities.Clear();
            }

            // Tìm tất cả các đối tượng Entity trong game và thêm vào danh sách
            Entity[] allEntities = FindObjectsByType<Entity>(FindObjectsSortMode.None);
            Entities.AddRange(allEntities);
        }

        // Hàm để tìm một Entity trong danh sách Entities dựa trên ID
        public Entity FindEntityById(string id)
        {
            foreach (Entity entity in Entities)
            {
                Debug.Log($"{entity.ID}");

                if (entity.ID == id)
                {

                    return entity;
                }
            }
            return null; // Trả về null nếu không tìm thấy
        }
    }
}

