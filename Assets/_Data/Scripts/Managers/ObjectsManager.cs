
using CuaHang;
using UnityEngine;

public class ObjectsManager : Singleton<ObjectsManager>
{
    [Header("OBJECTS MANAGER")]
    [SerializeField] ModuleDragItem moduleDragItem;

    public ModuleDragItem ModuleDragItem { get => moduleDragItem; set => moduleDragItem = value; }
}
