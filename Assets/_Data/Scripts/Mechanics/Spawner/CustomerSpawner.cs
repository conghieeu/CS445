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
        [SerializeField] List<Transform> _spawnPoint;
        [SerializeField] List<Transform> _dispawnPoints;

        [Header("Customer Spawner")]
        [SerializeField] float _baseSpawnInterval = 30.0f;    // Khoảng thời gian spawn cơ bản
        [SerializeField] float _randomRange = 10f;          // Khoảng thời gian ngẫu nhiên thêm vào
        [SerializeField] float _currentSpawnInterval = 10f;       // Thời gian spawn hiện tại
        [SerializeField] float _spawnTimer = 10f;                 // Bộ đếm thời gian cho spawn khách hàng

        [Header("Lounger Spawner")]
        [SerializeField] float _timeSpawnLounger = 5.0f;
        [SerializeField] float _randomRangeTimeSpawnLounger = 3f;
        [SerializeField] float _currentTimerLounger = 5;
        [SerializeField] float _spawnTimerLounger;

        CustomerPooler m_customerPooler;
        PlayerCtrl m_playerCtrl; 
            

        private void Awake()
        {
            m_playerCtrl = FindFirstObjectByType<PlayerCtrl>();
            m_customerPooler = FindFirstObjectByType<CustomerPooler>();

            _spawnTimer = _currentSpawnInterval;
            _spawnTimerLounger = _currentTimerLounger;
        }

        private void FixedUpdate()
        {
            SpawnLoungerOverTime();
            SpawnCustomerOverTime();
            AdjustSpawnRate(m_playerCtrl.Reputation);
        }

        /// <summary> Lấy một điểm ngẫu nhiên từ danh sách điểm thoát </summary>
        public Transform GetRandomOutPoint()
        {
            if (_dispawnPoints.Count > 0)
            {
                int randomIndex = Random.Range(0, _dispawnPoints.Count);
                return _dispawnPoints[randomIndex];
            }

            return null;
        }

        /// <summary> Spawn khách hàng muốn mua item </summary>
        private void SpawnCustomerOverTime()
        {
            _spawnTimer -= Time.fixedDeltaTime;

            if (_spawnTimer <= 0)
            {
                // Đặt lại bộ đếm thời gian với yếu tố ngẫu nhiên
                _spawnTimer = _currentTimerLounger + Random.Range(-_randomRange, _randomRange);

                if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
                {
                    SpawnCustomer();
                }
            }
        }

        private Customer SpawnCustomer()
        {
            int rCustomer = Random.Range(0, _customerPrefabs.Count);
            int rPoint = Random.Range(0, _spawnPoint.Count);

            Customer customer = m_customerPooler.GetOrCreateObjectPool(_customerPrefabs[rCustomer].TypeID, _spawnPoint[rPoint].position).GetComponent<Customer>();

            // Debug.Log($"spawn customer: {customer} Pos: {customer.transform.position}", customer);

            return customer;
        }

        [Command]
        /// <summary> Điều chỉnh tỷ lệ spawn dựa trên danh tiếng </summary>
        private void AdjustSpawnRate(float reputation)
        {
            float spawnRateMultiplier = 1.0f + (reputation / 100.0f); // Danh tiếng cao -> spawn nhanh hơn
            _currentSpawnInterval = _baseSpawnInterval / spawnRateMultiplier;
        }

        /// <summary> Spawn khách hàng muốn mua item </summary>
        private void SpawnLoungerOverTime()
        {
            _spawnTimerLounger -= Time.fixedDeltaTime;

            if (_spawnTimerLounger <= 0)
            {
                // Đặt lại bộ đếm thời gian với yếu tố ngẫu nhiên
                _spawnTimerLounger = _timeSpawnLounger + Random.Range(-_randomRangeTimeSpawnLounger, _randomRangeTimeSpawnLounger);

                if (_customerPrefabs.Count > 0 && _spawnPoint.Count > 0)
                {
                    Customer customer = SpawnCustomer();
                    customer.ListItemBuy.Clear();
                }
            }
        }
    }
}