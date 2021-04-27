using UnityEngine;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "MineCartPoints")]
    public class MineCartPointsAsset : ScriptableObject
    {
        public float pointsHudMultiplier = 100;
        
        public float pointsPerTime = 1;
        public float jumpMultiplier = 3;
    }
}