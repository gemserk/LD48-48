using UnityEngine;

namespace Scenes.Controls
{
    public class ParticleAttachPoint : MonoBehaviour
    {
        public GameObject particleSystemPrefab;

        public void Spawn()
        {
            if (particleSystemPrefab != null)
            {
                Instantiate(particleSystemPrefab, transform.position, Quaternion.identity);
            }
        }
    }
}