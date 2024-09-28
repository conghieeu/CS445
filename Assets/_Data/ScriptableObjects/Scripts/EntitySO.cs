using UnityEngine;

public enum Type
{
    Unknown = 0,
    Shelf,
    Parcel,
    Computer,
    Storage,
    Trash,
    Products,
    Staff,
    PottedPant,
    Customer,
}

public enum TypeID
{
    Unknown = 0,
    ComputerA,
    CustomerA,
    CustomerB,
    ShelfA,
    ShelfB,
    ShelfC,
    TableA,
    ParcelA,
    TrashCanA,
    StorageA,
    AppleA,
    MilkA,
    BananaA,
    PottedPantA,
    PottedPantB,
    PottedPantC,
    PottedPantD,
    StaffA,

}

[CreateAssetMenu(fileName = "ObjectSO", menuName = "ObjectSO", order = 0)]
public class EntitySO : ScriptableObject
{
    public string _name; // Tên của đối tượng và có thể thay đổi vì người dùng có thể dặt tên lại
    public TypeID _typeID; // đây như kiểu product type id của đối tượng, không thể thay đổi 
    public Type _type; // Kiểu của đối tượng và không thể thay đổi
}