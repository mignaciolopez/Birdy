using UnityEngine;

namespace Birdy.ClassicMode.Gameplay
{
    //[RequireComponent(typeof(Rigidbody2D))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float _impulse = 9.0f;
        private bool _jump;
        private Rigidbody2D _rb;
        private float _rotation;
        [SerializeField] private float _rotationMultipler = 3.0f;

        private void Start()
        {
            _jump = false;
            _rb = GetComponent<Rigidbody2D>();
            _rotation = 0f;
        }

        private void FixedUpdate()
        {
            if (_jump)
            {
                _rb.velocity = new Vector2(_rb.velocity.x, _impulse);
                _jump = false;
            }
        }

        private void Update()
        {
            if (!_jump && (Input.GetKeyDown(KeyCode.RightControl) || Input.GetKeyDown(KeyCode.LeftControl)))
            {
                _jump = true;
            }

            _rotation = _rb.velocity.y * _rotationMultipler;
            transform.rotation = Quaternion.Euler(new Vector3(0f, 0f, _rotation));
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            _rb.bodyType = RigidbodyType2D.Static;
        }
    }
}
