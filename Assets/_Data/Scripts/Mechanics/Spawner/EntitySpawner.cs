using UnityEngine;

public class EntitySpawner : GameBehavior
{
    public Transform SpawnerPoint;

    public Vector3 GetRamdomSpawnPos()
    {
        float size = 2f;
        float rx = UnityEngine.Random.Range(-size, size);
        float rz = UnityEngine.Random.Range(-size, size);

        Vector3 p = SpawnerPoint.position;

        return new Vector3(p.x + rx, p.y, p.z + rz);
    }
}
