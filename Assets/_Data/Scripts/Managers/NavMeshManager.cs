using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class NavMeshManager : Singleton<NavMeshManager>
{
    [Header("NAV MESH MANAGER")]
    [SerializeField] List<NavMeshSurface> _navMeshSurfaces = new List<NavMeshSurface>();

    private void OnEnable()
    {
        DataManager.ActionInitalizedData += OnInitalizedData;
    }

    private void OnDisable()
    {
        DataManager.ActionInitalizedData -= OnInitalizedData;
    }

    /// <summary> Bắt sự kiện sau khi các Load Value rồi </summary>
    public void OnInitalizedData()
    {
        RebuildNavMeshes();
    }

    public void RebuildNavMeshes()
    {
        foreach (NavMeshSurface navMeshSurface in _navMeshSurfaces)
        {
            if (navMeshSurface != null)
            {
                navMeshSurface.BuildNavMesh();
            }
        }
    }

    private void OnValidate()
    {
        _navMeshSurfaces.Clear();
        foreach (Transform child in transform)
        {
            NavMeshSurface surface = child.GetComponent<NavMeshSurface>();
            if (surface != null)
            {
                _navMeshSurfaces.Add(surface);
            }
        }
    }
}
