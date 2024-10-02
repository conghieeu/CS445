using UnityEngine;
using System.Collections.Generic;
using Unity.AI.Navigation;

public class NavMeshManager : GameBehavior
{
    [Header("NAV MESH MANAGER")]
    [SerializeField] List<NavMeshSurface> _navMeshSurfaces = new List<NavMeshSurface>();

    private void Start()
    {
        DataManager.ActionDataLoaded += RebuildNavMeshes;
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
