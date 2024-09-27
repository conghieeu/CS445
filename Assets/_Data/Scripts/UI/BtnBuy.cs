using System.Collections;
using System.Collections.Generic;
using CuaHang.Pooler;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace CuaHang.UI
{
    public class BtnBuy : GameBehavior
    {
        [Space]
        [SerializeField] TMP_Text _txtDescribe;
        [SerializeField] TMP_Text _txtPrice;
        [SerializeField] string _describe;
        [SerializeField] List<ItemSO> _items;
        [SerializeField] ItemSO _parcel; // nếu khi tạo cần parcel thì bỏ parcel vào đây

        Button _btnBuy;

        private void Start()
        {
            _btnBuy = GetComponent<Button>();
            _btnBuy.onClick.AddListener(BuyItem);
            _txtDescribe = GetChild<TMP_Text>("txtDescribe");
            _txtPrice = GetChild<TMP_Text>("txtPrice");

            // set gia

            // set mota
            _txtDescribe.text = _describe;
            _txtPrice.text = GetTotalPrice().ToString("F2");
        }

        public float GetTotalPrice()
        {
            float tongGia = 0f;

            foreach (ItemSO item in _items)
            {
                tongGia += item._priceDefault; // Cộng giá của từng mục vào tổng
            }

            return tongGia; // Trả về tổng giá
        }

        public void BuyItem()
        {
            if (_items.Count == 0)
            {
                Debug.LogWarning("btnButton mua này thiếu item SO yêu cầu", transform);
                return;
            }

            if (_items[0]._typeID == TypeID.StaffA)
            {
                Transform entity = StaffPooler.Instance.GetOrCreateObjectPool(_items[0]._typeID).transform;
                SetRandomPos(entity);
                return;
            }

            if (_parcel)
            {
                Item parcel = ItemPooler.Instance.GetOrCreateObjectPool(_parcel._typeID).GetComponent<Item>();
                parcel.CreateItemInSlot(_items);
                SetRandomPos(parcel.transform);
            }
            else
            {
                Item item = ItemPooler.Instance.GetOrCreateObjectPool(_items[0]._typeID).GetComponent<Item>();
                SetRandomPos(item.transform);
            }


        }

        public void SetRandomPos(Transform entity)
        {
            float size = 2f;
            float rx = UnityEngine.Random.Range(-size, size);
            float rz = UnityEngine.Random.Range(-size, size);

            Vector3 p = ItemPooler.Instance.ItemSpawnerPoint.position;

            entity.transform.position = new Vector3(p.x + rx, p.y, p.z + rz);
        }
    }
}
