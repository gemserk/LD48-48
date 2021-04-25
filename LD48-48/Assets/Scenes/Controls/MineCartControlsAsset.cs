using UnityEngine;

namespace Scenes.Controls
{
    [CreateAssetMenu(menuName = "MineCartControls")]
    public class MineCartControlsAsset : ScriptableObject
    {
        public float tiltAngle = 15.0f;
        public float jumpTiltXAngle = 25.0f;

        public float jumpStartTreshold = 0.1f;

        public Vector3 jumpForce = new Vector3(0, 100, 0);
        public ForceMode forceMode = ForceMode.Force;

        public Vector3 moveWhileJumpForce = new Vector3(0,0, 100);
        
        public float jumpDuration = 0.25f;

        public float jumpForwardSpeedFactor = 0.5f;
        public float jumpUpVelocityFactor = 1.0f;
        public float jumpTiltVelocityFactor = 5.0f;

        public float timeToActivateRigidBody = 0.25f;
        
        public float overrideRigidBodyMass = 5.0f;
    }
}