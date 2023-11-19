using UnityEngine;

namespace Birdy.Shared.UI
{
    [RequireComponent(typeof(Camera))]
    [ExecuteAlways]
    public class AspectRatioCameraFitter : MonoBehaviour
    {
        [SerializeField] private Camera cam;

        private readonly Vector2 targetAspectRatio = new(9, 16);
        private readonly Vector2 rectCenter = new(0.5f, 0.5f);

        private Vector2 lastResolution;

        private void OnValidate()
        {
            cam = cam != null ? cam : GetComponent<Camera>();
        }

        public void LateUpdate()
        {
            var currentScreenResolution = new Vector2(Screen.width, Screen.height);

            // Don't run all the calculations if the screen resolution has not changed
            if (lastResolution != currentScreenResolution)
            {
                //CalculateCameraRect(currentScreenResolution);
                CalculateCameraOrthographicSize();
            }

            lastResolution = currentScreenResolution;
        }

        private void CalculateCameraRect(Vector2 currentScreenResolution)
        {
            var normalizedAspectRatio = targetAspectRatio / currentScreenResolution;
            var size = normalizedAspectRatio / Mathf.Max(normalizedAspectRatio.x, normalizedAspectRatio.y);
            cam.rect = new Rect(default, size) { center = rectCenter };
        }

        private void CalculateCameraOrthographicSize()
        {
            float relation = Screen.height / Screen.width;

            switch (relation)
            {
                case >= 2f:
                    cam.orthographicSize = 6f;
                    break;
                default:
                    cam.orthographicSize = 4.6f;
                    break;
            }
        }
    }
}