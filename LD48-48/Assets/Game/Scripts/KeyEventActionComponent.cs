using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class KeyEventActionComponent : MonoBehaviour
{
    public Key keyCode;
    public UnityEvent keyDownEvent;
    public void Update()
    {
        if (Keyboard.current[keyCode].wasPressedThisFrame)
        {
            keyDownEvent.Invoke();
        }
    }
}
