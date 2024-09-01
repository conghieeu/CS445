using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerTest : MonoBehaviour
{ 
    public GamePCInput gamePCInput;

    private void Start()
    {
        gamePCInput = new ();
    }

    private void FixedUpdate()
    {  
        Debug.Log(gamePCInput.MovementInput());
    }
}

