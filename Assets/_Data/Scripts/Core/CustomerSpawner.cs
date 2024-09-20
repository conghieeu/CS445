using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using UnityEngine;

namespace CuaHang.Core
{
    public class CustomerSpawner : MonoBehaviour
    {
        [SerializeField] List<Customer> _customerPrefabs;
        [SerializeField] List<Transform> _spawnPoint;
        [SerializeField] float _baseSpawnInterval = 50.0f;    // Khoảng thời gian spawn cơ bản
        [SerializeField] float _randomRange = 2.0f;          // Khoảng thời gian ngẫu nhiên thêm vào
        [SerializeField] float _currentSpawnInterval;       // Thời gian spawn hiện tại
        [SerializeField] float _spawnTimer;                 // Bộ đếm thời gian cho spawn khách hàng

        private void Start()
        {
            _spawnTimer = _currentSpawnInterval;
        }

        private void FixedUpdate()
        {
            SpawnCustomerOverTime();
        }

        private void OnEnable()
        {
            PlayerCtrl.ActionReputationChange += AdjustSpawnRate;
        }

        private void OnDisable()
        {
            PlayerCtrl.ActionReputationChange -= AdjustSpawnRate;
        }

        /// <summary> Điều chỉnh tỷ lệ spawn dựa trên danh tiếng </summary>
        private void AdjustSpawnRate(float reputation)
        {
            float spawnRateMultiplier = 1.0f + (reputation / 100.0f); // Danh tiếng cao -> spawn nhanh hơn
            _currentSpawnInterval = _baseSpawnInterval / spawnRateMultiplier;
        }

        /// <summary> Spawn khách hàng theo thời gian </summary>
        private void SpawnCustomerOverTime()
        {
            _spawnTimer -= Time.fixedDeltaTime;

            if (_spawnTimer <= 0)
            {
                if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
                {
                    int rCustomer = Random.Range(0, _customerPrefabs.Count);
                    int rPoint = Random.Range(0, _spawnPoint.Count);

                    // spawn customer
                    Customer cus = CustomerPooler.Instance.GetOrCreateObjectPool(_customerPrefabs[rCustomer].TypeID).GetComponent<Customer>();
                    cus.transform.position = _spawnPoint[rPoint].position;
                }

                // Đặt lại bộ đếm thời gian với yếu tố ngẫu nhiên
                _spawnTimer = _currentSpawnInterval + Random.Range(-_randomRange, _randomRange);
            }
        }
    }

}