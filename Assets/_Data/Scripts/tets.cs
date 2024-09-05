using UnityEngine;
using UnityEngine.UI;

public class tets : MonoBehaviour
{
    GraphicRaycaster _uiRaycaster;

    protected void Awake()
    { 
        _uiRaycaster = GetComponentInChildren<GraphicRaycaster>();
    }

}

