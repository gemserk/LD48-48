using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyEventActionComponent : MonoBehaviour
{
    public KeyCode keyCode;
    public UnityEvent keyDownEvent;
    public void Update()
    {
        if (Input.GetKeyDown(keyCode))
        {
            keyDownEvent.Invoke();
        }
    }
}
