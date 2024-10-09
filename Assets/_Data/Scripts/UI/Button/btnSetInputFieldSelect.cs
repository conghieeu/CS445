using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class btnSetInputFieldSelect : GameBehavior
{
    public TMP_InputField InfSelect;

    private void Start()
    {
        GetComponent<Button>().onClick.AddListener(OnInputFieldSelect);
        InfSelect.onEndEdit.AddListener(OnInputFieldEndEdit);
    }

    private void OnInputFieldEndEdit(string arg0)
    {
        Debug.Log(arg0);
        InfSelect.enabled = false; 
    }

    private void OnInputFieldSelect()
    {
        InfSelect.enabled = true;
        InfSelect.ActivateInputField();
    }
}
