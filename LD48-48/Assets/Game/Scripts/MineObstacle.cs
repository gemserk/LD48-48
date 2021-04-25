using UnityEngine;

namespace Game.Scripts
{
    public class MineObstacle : MonoBehaviour
    {
        public Collider collider;
        
        public void DisableCollisions()
        {
            collider.enabled = false;
        }
    }
}