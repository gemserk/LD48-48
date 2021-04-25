using UnityEngine;

namespace Game.Scripts
{
    public class ParticleAttachPoint : MonoBehaviour
    {
        public GameObject particleSystemPrefab;

        // public bool asChild = true;

        public void Spawn()
        {
            if (particleSystemPrefab != null)
            {
                Instantiate(particleSystemPrefab, transform);
            }
        }
    }
}