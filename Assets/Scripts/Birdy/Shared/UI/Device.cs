using System;
using UnityEngine;

namespace Birdy.Shared.UI
{
    [ExecuteAlways]
    public class Device : MonoBehaviour
    {
        public event Action<Resolution> OnResolutionChange;
        public event Action<DeviceOrientation> OnOrientationChange;

        private Resolution _resolution;
        private DeviceOrientation _orientation;

        public Resolution GetResolution() { return _resolution; }

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

            _resolution = new Resolution();
            _resolution.width = Screen.width;
            _resolution.height = Screen.height;

            _orientation = Input.deviceOrientation;
        }

        private void Update()
        {
            // Check for a Resolution Change
            if (_resolution.width != Screen.width || _resolution.height != Screen.height)
            {
                Debug.Log($"{name} OnResolutionChange: {_resolution.width} -> {Screen.width} | {_resolution.height} -> {Screen.height}");
                _resolution.width = Screen.width;
                _resolution.height = Screen.height;
                OnResolutionChange?.Invoke(_resolution);
            }

            // Check for an Orientation Change
            switch (Input.deviceOrientation)
            {
                case DeviceOrientation.Unknown:            // Ignore
                case DeviceOrientation.FaceUp:            // Ignore
                case DeviceOrientation.FaceDown:        // Ignore
                    break;
                default:
                    if (_orientation != Input.deviceOrientation)
                    {
                        Debug.Log($"{name} OnOrientationChange");
                        _orientation = Input.deviceOrientation;
                        OnOrientationChange?.Invoke(_orientation);
                    }
                    break;
            }
        }
    }
}