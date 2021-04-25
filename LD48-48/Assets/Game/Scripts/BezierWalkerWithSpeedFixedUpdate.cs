using BezierSolution;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class BezierWalkerWithSpeedFixedUpdate : BezierWalker
    {
        public BezierSpline spline;
        public TravelMode travelMode;

        public float speed = 5f;
        [SerializeField]
        [Range( 0f, 1f )]
        private float m_normalizedT = 0f;

        public override BezierSpline Spline => spline;

        public override float NormalizedT
        {
            get => m_normalizedT;
            set => m_normalizedT = value;
        }

        //public float movementLerpModifier = 10f;
        public float rotationLerpModifier = 10f;

        public LookAtMode lookAt = LookAtMode.Forward;

        private bool isGoingForward = true;
        public override bool MovingForward => speed > 0f == isGoingForward;

        public UnityEvent onPathCompleted = new UnityEvent();
        private bool onPathCompletedCalledAt1 = false;
        private bool onPathCompletedCalledAt0 = false;

        private void FixedUpdate()
        {
            Execute( Time.deltaTime );
        }

        public override void Execute( float deltaTime )
        {
            var targetSpeed = ( isGoingForward ) ? speed : -speed;

            var targetPos = spline.MoveAlongSpline( ref m_normalizedT, targetSpeed * deltaTime );

            transform.position = targetPos;
            //transform.position = Vector3.Lerp( transform.position, targetPos, movementLerpModifier * deltaTime );

            var movingForward = MovingForward;

            if( lookAt == LookAtMode.Forward )
            {
                var tuple = spline.GetNearestPointIndicesTo( m_normalizedT );
                Quaternion targetRotation;
                if( movingForward )
                    targetRotation = Quaternion.LookRotation( tuple.GetTangent(), tuple.GetNormal() );
                else
                    targetRotation = Quaternion.LookRotation( -tuple.GetTangent(), tuple.GetNormal() );

                transform.rotation = Quaternion.Lerp( transform.rotation, targetRotation, rotationLerpModifier * deltaTime );
            }
            else if( lookAt == LookAtMode.SplineExtraData )
                transform.rotation = Quaternion.Lerp( transform.rotation, spline.GetExtraData( m_normalizedT, extraDataLerpAsQuaternionFunction ), rotationLerpModifier * deltaTime );

            if( movingForward )
            {
                if( m_normalizedT >= 1f )
                {
                    if( travelMode == TravelMode.Once )
                        m_normalizedT = 1f;
                    else if( travelMode == TravelMode.Loop )
                        m_normalizedT -= 1f;
                    else
                    {
                        m_normalizedT = 2f - m_normalizedT;
                        isGoingForward = !isGoingForward;
                    }

                    if( !onPathCompletedCalledAt1 )
                    {
                        onPathCompletedCalledAt1 = true;
#if UNITY_EDITOR
                        if( UnityEditor.EditorApplication.isPlaying )
#endif
                            onPathCompleted.Invoke();
                    }
                }
                else
                {
                    onPathCompletedCalledAt1 = false;
                }
            }
            else
            {
                if( m_normalizedT <= 0f )
                {
                    if( travelMode == TravelMode.Once )
                        m_normalizedT = 0f;
                    else if( travelMode == TravelMode.Loop )
                        m_normalizedT += 1f;
                    else
                    {
                        m_normalizedT = -m_normalizedT;
                        isGoingForward = !isGoingForward;
                    }

                    if( !onPathCompletedCalledAt0 )
                    {
                        onPathCompletedCalledAt0 = true;
#if UNITY_EDITOR
                        if( UnityEditor.EditorApplication.isPlaying )
#endif
                            onPathCompleted.Invoke();
                    }
                }
                else
                {
                    onPathCompletedCalledAt0 = false;
                }
            }
        }
    }
}