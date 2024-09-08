using UnityEngine;

public class SpawnManager : Singleton<SpawnManager>
{
    [Header("SPAWNER MANAGER")]
    [SerializeField] Transform _itemSpawner;

    public Transform _ItemSpawner { get => _itemSpawner; }
}

