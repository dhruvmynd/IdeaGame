using UnityEngine;

namespace CarControllerwithShooting
{
    public class FollowTargetCamera : MonoBehaviour
    {
        public Transform Target;
        public Transform CarCameraTarget;
        public float PositionFolowForce = 5f;
        public float RotationFolowForce = 5f;

        //Dhruv
        void Start()
        {
            // Subscribe to the missile fired event (Dhruv)
            GunController.OnMissileFired += HandleMissileFired;

        }

        void FixedUpdate()
        {
            if (Target != null)
            {
                var vector = Vector3.forward;
                var dir = Target.rotation * Vector3.forward;
                dir.y = 0f;
                if (dir.magnitude > 0f) vector = dir / dir.magnitude;

                transform.position = Vector3.Lerp(transform.position, Target.position, PositionFolowForce * Time.deltaTime);
                transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vector), RotationFolowForce * Time.deltaTime);
            }
        }
        //Dhruv
        private void HandleMissileFired(Transform missileTransform)
        {
            if (missileTransform != null)
            {
                // Change the target to the missile (Dhruv)
                Target = missileTransform;
            }
            else
            {
                // Reset the target back to the car (Dhruv)
                Target = CarCameraTarget;
            }
        }
        //Dhruv
        void OnDestroy()
        {
            // Unsubscribe from the event when the script is destroyed (Dhruv)
            GunController.OnMissileFired -= HandleMissileFired;
        }
    }
}
