using System;
using UnityEngine;

namespace Game.Scripts
{
    public class LightBeatController : MonoBehaviour
    {
        public Light light;
        private float originalLight;
        public void Awake()
        {
            this.light = this.GetComponentInChildren<Light>();
            this.originalLight = this.light.intensity;
        }

        public void Change(float factor)
        {
            this.light.intensity = factor * originalLight;
        }
    }
}