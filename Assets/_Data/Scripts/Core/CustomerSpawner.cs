using System.Collections.Generic;
using CuaHang.AI;
using CuaHang.Pooler;
using QFSW.QC;
using UnityEngine;

namespace CuaHang.Core
{
    public class CustomerSpawner : GameBehavior
    {
        [Header("CUSTOMER SPAWNER")]
        [SerializeField] List<Customer> _customerPrefabs;

        [Header("Customer Spawner")]
        [SerializeField] List<Transform> _spawnPoint;
        [SerializeField] float _baseSpawnInterval = 30.0f;    // Khoảng thời gian spawn cơ bản
        [SerializeField] float _randomRange = 10f;          // Khoảng thời gian ngẫu nhiên thêm vào
        [SerializeField] float _currentSpawnInterval = 10f;       // Thời gian spawn hiện tại
        [SerializeField] float _spawnTimer = 10f;                 // Bộ đếm thời gian cho spawn khách hàng

        [Header("Lounger Spawner")]
        [SerializeField] List<Transform> _dispawnPoints;
        [SerializeField] float _timeSpawnLounger = 5.0f;
        [SerializeField] float _randomRangeTimeSpawnLounger = 3f;
        [SerializeField] float _spawnTimerLounger;

        private void FixedUpdate()
        {
            SpawnCustomerOverTime();
            AdjustSpawnRate(PlayerCtrl.Instance.Reputation);
            SpawnLounger();
        }

        [Command]
        /// <summary> Điều chỉnh tỷ lệ spawn dựa trên danh tiếng </summary>
        private void AdjustSpawnRate(float reputation)
        {
            float spawnRateMultiplier = 1.0f + (reputation / 100.0f); // Danh tiếng cao -> spawn nhanh hơn
            _currentSpawnInterval = _baseSpawnInterval / spawnRateMultiplier;
        }

        /// <summary> Spawn khách hàng muốn mua item </summary>
        private void SpawnLounger()
        {
            _spawnTimerLounger -= Time.fixedDeltaTime;

            if (_spawnTimerLounger <= 0)
            {
                // Đặt lại bộ đếm thời gian với yếu tố ngẫu nhiên
                _spawnTimerLounger = _timeSpawnLounger + Random.Range(-_randomRangeTimeSpawnLounger, _randomRangeTimeSpawnLounger);

                if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
                {
                    int rCustomer = Random.Range(0, _customerPrefabs.Count);
                    int rPoint = Random.Range(0, _spawnPoint.Count);

                    // spawn customer
                    Customer cus = CustomerPooler.Instance.GetOrCreateObjectPool(_customerPrefabs[rCustomer].TypeID).GetComponent<Customer>();
                    cus.gameObject.SetActive(false);
                    cus.transform.position = _spawnPoint[rPoint].position;
                    cus.gameObject.SetActive(true);
                    cus.ListItemBuy.Clear();
                }
            }
        }

        /// <summary> Spawn khách hàng muốn mua item </summary>
        private void SpawnCustomerOverTime()
        {
            _spawnTimer -= Time.fixedDeltaTime;

            if (_spawnTimer <= 0)
            {
                // Đặt lại bộ đếm thời gian với yếu tố ngẫu nhiên
                _spawnTimer = _currentSpawnInterval + Random.Range(-_randomRange, _randomRange);

                if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
                {
                    int rCustomer = Random.Range(0, _customerPrefabs.Count);
                    int rPoint = Random.Range(0, _spawnPoint.Count);

                    // spawn customer
                    Customer cus = CustomerPooler.Instance.GetOrCreateObjectPool(_customerPrefabs[rCustomer].TypeID).GetComponent<Customer>();
                    cus.transform.position = _spawnPoint[rPoint].position;
                }
            }
        }
    }

}