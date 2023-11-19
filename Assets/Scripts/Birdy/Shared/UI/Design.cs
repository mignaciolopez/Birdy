using System;
using UnityEngine;

namespace Birdy.Shared.UI
{
    public class Design : MonoBehaviour
    {
        public event Action<Resolution> OnResolutionChange;

        [SerializeField][Min(0)] private Vector2Int _designSize;
        private Resolution _designResolution;
        [SerializeField] private Device _device;
        public Camera ActiveCamera;

        [HideInInspector] public Vector2 ViewDesignSize;
        [HideInInspector] public Vector4 ViewDesignBorders;

        [HideInInspector] public Vector2 ViewSize;
        [HideInInspector] public Vector4 ViewBorders;

        private void OnValidate()
        {
            Init();
        }

        private void OnEnable()
        {
            Init();
        }

        private void Init()
        {
            Debug.Log($"{this}");

            _designResolution.width = _designSize.x;
            _designResolution.height = _designSize.y;

            if (_designResolution.width <= 0 || _designResolution.height <= 0)
            {
                _designSize = new Vector2Int(1080, 1920);

                Debug.LogWarning(
                    $"Invalid Design Resolution {_designResolution} \n" +
                    $"Assuming {_designSize.x}x{_designSize.y}");

                _designResolution.width = _designSize.x;
                _designResolution.height = _designSize.y;
            }

            if (ActiveCamera == null)
                ActiveCamera = Camera.main;

            if (ActiveCamera == null || !ActiveCamera.orthographic)
            {
                Debug.LogError($"No Camera Found on Scene for {gameObject.name}");
                return;
            }

            if (_device == null)
            {
                _device = FindAnyObjectByType<Device>();
                if (_device == null)
                {
                    Debug.LogError($"There is no gameObject with Device Component attached!");
                    return;
                }
            }

            ViewDesignSize = GetViewDesignSize();
            ViewDesignBorders = GetViewDesignBorders();
            ViewSize = ViewDesignSize;
            ViewBorders = ViewDesignBorders;

            _device.OnResolutionChange += Device_OnResolutionChange;
        }

        private void OnDestroy()
        {
            _device.OnResolutionChange -= Device_OnResolutionChange;
        }

        private Vector2 GetViewDesignSize()
        {
            var viewDesignSize = new Vector2
            {
                x = ActiveCamera.orthographicSize * 2.0f * _designResolution.width / _designResolution.height,
                y = ActiveCamera.orthographicSize * 2.0f
            };

            Debug.Log(
                $"Design Resolution: {_designResolution.width}x{_designResolution.height} \n" +
                $"Camera Orthographic Size: {ActiveCamera.orthographicSize} \n" +
                $"Calculated View Design Size: {viewDesignSize}");

            return viewDesignSize;
        }

        private Vector2 GetViewSize()
        {
            var viewSize = new Vector2
            {
                x = ActiveCamera.orthographicSize * 2.0f * _device.GetResolution().width / _device.GetResolution().height,
                y = ActiveCamera.orthographicSize * 2.0f
            };

            Debug.Log(
                $"Device Resolution: {_device.GetResolution().width}x{_device.GetResolution().height} \n" +
                $"Camera Orthographic Size: {ActiveCamera.orthographicSize} \n" +
                $"Calculated View Size: {viewSize}");

            return viewSize;
        }

        private Vector4 GetViewDesignBorders()
        {
            var viewDesignBorders = new Vector4
            {
                x = ActiveCamera.transform.position.x - (ViewDesignSize.x / 2f), //Left
                y = ActiveCamera.transform.position.x + (ViewDesignSize.x / 2f), //Right
                z = ActiveCamera.transform.position.y + ActiveCamera.orthographicSize, //Top
                w = ActiveCamera.transform.position.y - ActiveCamera.orthographicSize  //Bottom
            };

            Debug.Log($"View Design Borders: {viewDesignBorders}");

            return viewDesignBorders;
        }

        private Vector4 GetViewBorders()
        {
            var viewBorders = new Vector4
            {
                x = ActiveCamera.transform.position.x - (ViewSize.x / 2f),          //Left
                y = ActiveCamera.transform.position.x + (ViewSize.x / 2f),          //Right
                z = ActiveCamera.transform.position.y + ActiveCamera.orthographicSize,    //Top
                w = ActiveCamera.transform.position.y - ActiveCamera.orthographicSize     //Bottom
            };

            Debug.Log($"View Borders: {viewBorders}");

            return viewBorders;
        }

        private void Device_OnResolutionChange(Resolution resolution)
        {
            ViewSize = GetViewSize();
            ViewBorders = GetViewBorders();

            OnResolutionChange?.Invoke(resolution);
        }
    }
}