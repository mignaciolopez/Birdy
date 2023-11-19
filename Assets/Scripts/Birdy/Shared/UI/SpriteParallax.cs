using UnityEngine;

namespace Birdy.Shared.UI
{
    public class SpriteParallax : MonoBehaviour
    {
        [Tooltip("1 Means no parallax effect. The Higher the value the slower the background moves.")]
        public Vector2 Parallax;
        public float Speed;
        private enum UpdateMode
        {
            FixedUpdate,
            Update,
            LateUpdate
        }
        [SerializeField] private UpdateMode _updateMode = UpdateMode.FixedUpdate;
        private enum ParallaxType
        {
            TransformBased,
            TimeBased,
            SpeedBased
        }
        [SerializeField] private ParallaxType _parallaxType = ParallaxType.TransformBased;

        private SpriteRenderer _spriteRenderer;
        private Vector2 _offset;
        private Vector2 _initialOffset;
        private float _timeElapsed;

        void OnEnable()
        {
            if (!TryGetComponent(out _spriteRenderer))
                Debug.LogError($"{gameObject} has no SpriteRenderer Component");

            _initialOffset = _spriteRenderer.material.mainTextureOffset;
        }

        private void OnDisable()
        {
            _spriteRenderer.material.mainTextureOffset = _initialOffset;
        }

        private void UpdateOffset()
        {
            switch (_parallaxType)
            {
                case ParallaxType.TransformBased:
                    {
                        if (Parallax.x != 0)
                            _offset.x = transform.position.x / _spriteRenderer.size.x / Parallax.x;

                        if (Parallax.y != 0)
                            _offset.y = transform.position.y / _spriteRenderer.size.y / Parallax.y;


                        if (_spriteRenderer.material != null)
                            _spriteRenderer.material.mainTextureOffset = _offset;

                        break;
                    }
                case ParallaxType.SpeedBased:
                    {
                        Parallax.x = 1.0f / Speed;
                        _timeElapsed += Time.deltaTime;
                        if (_spriteRenderer.material != null)
                        {
                            if (Parallax.x != 0)
                                _offset.x = _timeElapsed / _spriteRenderer.sprite.texture.width * _spriteRenderer.sprite.pixelsPerUnit / Parallax.x;

                            if (Parallax.y != 0)
                                _offset.y = _timeElapsed / _spriteRenderer.sprite.texture.height * _spriteRenderer.sprite.pixelsPerUnit / Parallax.y;

                            _spriteRenderer.material.mainTextureOffset = _offset;
                        }
                        break;
                    }
                case ParallaxType.TimeBased:
                        _timeElapsed += Time.deltaTime;
                        if (_spriteRenderer.material != null)
                        {
                            if (Parallax.x != 0)
                                _offset.x = _timeElapsed / _spriteRenderer.sprite.texture.width * _spriteRenderer.sprite.pixelsPerUnit / Parallax.x;

                            if (Parallax.y != 0)
                                _offset.y = _timeElapsed / _spriteRenderer.sprite.texture.height * _spriteRenderer.sprite.pixelsPerUnit / Parallax.y;

                            _spriteRenderer.material.mainTextureOffset = _offset;
                        }
                        break;
                default:
                    Debug.LogError($"Type {_parallaxType} not handled");
                    break;
            }
        }

        private void FixedUpdate()
        {
            if (_updateMode == UpdateMode.FixedUpdate)
                UpdateOffset();
        }

        private void Update()
        {
            if (_updateMode == UpdateMode.Update)
                UpdateOffset();
        }

        private void LateUpdate()
        {
            if (_updateMode == UpdateMode.LateUpdate)
                UpdateOffset();
        }
    }
}