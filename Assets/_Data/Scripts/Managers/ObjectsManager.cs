
using CuaHang;
using CuaHang.Core;
using UnityEngine;

public class ObjectsManager : Singleton<ObjectsManager>
{
    [Header("OBJECTS MANAGER")]
    [SerializeField] ModuleDragItem moduleDragItem;
    [SerializeField] RaycastCursor raycastCursor;
    [SerializeField] NavMeshManager navMeshManager;
    [SerializeField] CameraControl cameraControl;
    [SerializeField] GameSettings gameSettings;
    [SerializeField] CustomerSpawner customerSpawner;

    public ModuleDragItem ModuleDragItem { get => moduleDragItem; private set => moduleDragItem = value; }
    public RaycastCursor RaycastCursor { get => raycastCursor; private set => raycastCursor = value; }
    public NavMeshManager NavMeshManager { get => navMeshManager; set => navMeshManager = value; }
    public CameraControl CameraControl { get => cameraControl; set => cameraControl = value; }
    public GameSettings GameSettings { get => gameSettings; set => gameSettings = value; }
    public CustomerSpawner CustomerSpawner { get => customerSpawner; set => customerSpawner = value; }

    private void OnValidate()
    {
        cameraControl = FindFirstObjectByType<CameraControl>();
        GameSettings = FindFirstObjectByType<GameSettings>();
        RaycastCursor = FindFirstObjectByType<RaycastCursor>();
        NavMeshManager = FindFirstObjectByType<NavMeshManager>();
        //ModuleDragItem = FindFirstObjectByType<ModuleDragItem>();
        CustomerSpawner = FindFirstObjectByType<CustomerSpawner>();
    }
}
