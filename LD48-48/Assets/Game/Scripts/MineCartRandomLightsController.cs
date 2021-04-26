using System;
using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts
{
    public class PingPongAnimation
    {
        public float animationSpeed = 1.0f;
        public float time = 0.0f;
        
        private float timeDirection = 1.0f;

        public void Update(float dt)
        {
            time += Time.deltaTime * timeDirection * animationSpeed;

            if (time > 1.0f)
            {
                timeDirection = -1f;
            }

            if (time < 0)
            {
                timeDirection = 1.0f;
            }
        }
    }
    
    public class MineCartRandomLightsController : MonoBehaviour
    {
        public Gradient lightColors;

        public List<Light> lights;

        public float animationSpeed = 1;

        private PingPongAnimation animation = new PingPongAnimation
        {
            animationSpeed = 1
        };
        
        private void LateUpdate()
        {
            animation.animationSpeed = animationSpeed;
            
            animation.Update(Time.deltaTime);
            var color = lightColors.Evaluate(animation.time);

            foreach (var light in lights)
            {
                light.color = color;
            }
        }
    }
}