using UnityEngine;
using UnityEngine.Serialization;

namespace Game.Scripts
{
    [CreateAssetMenu(menuName = "MineCartControls")]
    public class MineCartControlsAsset : ScriptableObject
    {
        public float tiltAngle = 15.0f;
        public float forwardTiltAngle = 10.0f;
        
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

        [FormerlySerializedAs("minTravelSpeed")] 
        public float slopeMinTravelSpeed = 5.0f;
        
        [FormerlySerializedAs("maxTravelSpeed")] 
        public float slopeMaxTravelSpeed = 30.0f;
        
        public float tiltMinTravelSpeedMultiplier = 0.5f;
        public float tiltMaxTravelSpeedMultiplier = 1.5f;
     
        // public AnimationCurve trackTravelSpeed;
    }
}