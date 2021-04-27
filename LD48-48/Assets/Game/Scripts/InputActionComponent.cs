using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class InputActionComponent : MonoBehaviour
    {
        public InputActionReference action;
        public UnityEvent actionEvent;
        public void Update()
        {
            if (action.action.triggered)
            {
                actionEvent.Invoke();
            }
        }
    }
}