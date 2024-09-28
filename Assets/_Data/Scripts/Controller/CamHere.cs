
using UnityEngine;

namespace CuaHang
{
    public class CamHere : MonoBehaviour
    {
        public bool _isOnCamHere;
        public float _camSize;

        // camera hãy tập trung vào đây
        public void SetCamFocusHere(Camera cam)
        {
            if (cam)
            {
                cam.orthographicSize = _camSize;
                cam.transform.position = transform.position;
                cam.transform.rotation = transform.rotation;
            }
        }
    }
}
