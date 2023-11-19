using UnityEngine;

namespace Birdy.Shared.UI
{
    [ExecuteAlways]
    public class AssetResizer : MonoBehaviour
    {
        [SerializeField] private Unity.Mathematics.bool2 _isReSizing;
        [SerializeField] private Unity.Mathematics.bool2 _isRePositioning;

        private SpriteRenderer _spriteRenderer;
        private Vector4 _positionPercents;

        [SerializeField] private Design _design;

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

            if (_design == null)
            {
                _design = FindAnyObjectByType<Design>();
                if (_design == null)
                {
                    Debug.LogError($"There is no gameObject with Device Component attached!");
                    return;
                }
            }

            if ((_isReSizing.x || _isReSizing.y) && !TryGetComponent(out _spriteRenderer))
            {
                Debug.LogError($"{gameObject.name} has no SpriteRenderer Component");
                return;
            }

            _positionPercents = new Vector4
            {
                x = (transform.position.x - _design.ViewDesignBorders.x) * 100f / _design.ViewDesignSize.x,
                y = (_design.ViewDesignBorders.y - transform.position.x) * 100f / _design.ViewDesignSize.x,
                z = (_design.ViewDesignBorders.z - transform.position.y) * 100f / _design.ViewDesignSize.y,
                w = (transform.position.y - _design.ViewDesignBorders.w) * 100f / _design.ViewDesignSize.y
            };
            Debug.Log($"Position Percents: {_positionPercents}");

            _design.OnResolutionChange += Design_OnResolutionChange;
        }

        private void OnDestroy()
        {
            _design.OnResolutionChange -= Design_OnResolutionChange;
        }

        private void Design_OnResolutionChange(Resolution resolution)
        {
            //Sizeing
            if (_spriteRenderer != null)
            {
                _spriteRenderer.size = new Vector2
                {
                    x = _isReSizing.x ? _design.ViewSize.x : _spriteRenderer.size.x,
                    y = _isReSizing.y ? _design.ViewSize.y : _spriteRenderer.size.y
                };
            }

            //Positioning
            float x = transform.position.x;
            float y = transform.position.y;

            if (_isRePositioning.x)
            {
                if (_positionPercents.x < 50.0f)
                {
                    x = _design.ViewBorders.x + _design.ViewSize.x * (_positionPercents.x / 100f);
                }
                else if (_positionPercents.x > 50.0f)
                {
                    x = _design.ViewBorders.y - _design.ViewSize.x * (_positionPercents.y / 100f);
                }
            }

            if (_isRePositioning.y)
            {
                if (_positionPercents.z > 50.0f)
                {
                    y = _design.ViewBorders.z - _design.ViewSize.y * (_positionPercents.z / 100f);
                }
                else if (_positionPercents.w < 50.0f)
                {
                    y = _design.ViewBorders.w + _design.ViewSize.y * (_positionPercents.w / 100f);
                }
            }

            transform.position = new Vector3(x, y, transform.position.z);
        }
    }
}