using UnityEngine;

namespace CarControllerwithShooting
{
    public class FollowTargetCamera : MonoBehaviour
    {
        public Transform Target;
        public Transform CarCameraTarget;
        public float DistanceBehind = 10f; // Static distance behind the car
        public float BaseDistanceAbove = 5f; // Base distance above the car
        public float PositionFollowForce = 5f;
        public float RotationFollowForce = 5f;
        public float ZoomSensitivity = 10f;
        public float MinFOV = 15f;
        public float MaxFOV = 90f;
        public float MinDistanceAbove = 2f; // Minimum distance above the car
        public float MaxDistanceAbove = 15f; // Maximum distance above the car

        private Camera childCamera;

        void Start()
        {
            childCamera = GetComponentInChildren<Camera>();
            GunController.OnMissileFired += HandleMissileFired;
        }

        void FixedUpdate()
        {
            if (Target != null)
            {
                // Handle mouse scroll input for zooming
                float scroll = Input.GetAxis("Mouse ScrollWheel");
                if (childCamera != null)
                {
                    childCamera.fieldOfView -= scroll * ZoomSensitivity;
                    childCamera.fieldOfView = Mathf.Clamp(childCamera.fieldOfView, MinFOV, MaxFOV);

                    // Adjust distance above based on FOV
                    float fovRatio = (childCamera.fieldOfView - MinFOV) / (MaxFOV - MinFOV);
                    float distanceAbove = Mathf.Lerp(MinDistanceAbove, MaxDistanceAbove, fovRatio);

                    // Calculate the desired position of the camera
                    Vector3 desiredPosition = Target.position - Target.forward * DistanceBehind + Vector3.up * distanceAbove;

                    // Interpolate towards the desired position to smooth the movement
                    transform.position = Vector3.Lerp(transform.position, desiredPosition, PositionFollowForce * Time.deltaTime);

                    // Always look at the target
                    transform.LookAt(Target.position);
                }
            }
        }

        private void HandleMissileFired(Transform missileTransform)
        {
            if (missileTransform != null)
            {
                Target = missileTransform;
            }
            else
            {
                Target = CarCameraTarget;
            }
        }

        void OnDestroy()
        {
            GunController.OnMissileFired -= HandleMissileFired;
        }
    }
}
