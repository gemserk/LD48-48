using UnityEngine;

namespace Game.Scripts
{
    public class TrackGeneratorStrategy : MonoBehaviour
    {
        public float probabiltiy;
        
        public float stepDistanceMin;
        public float stepDistanceMax;
    
        public float descentPerStep;
    
        public Vector2 maxRandomOffset;
        public Vector2 minRandomOffset;
    }
}