using Unity.Mathematics;
using UnityEngine;

namespace Birdy.Shared
{
    public class FollowObject : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private Vector3 _offset;
        [SerializeField] private bool3 _axis;
        private void LateUpdate()
        {
            Vector3 pos = Vector3.zero;

            pos.x = _axis.x ? _target.position.x + _offset.x : transform.position.x;
            pos.y = _axis.y ? _target.position.y + _offset.y : transform.position.y;
            pos.z = _axis.z ? _target.position.z + _offset.z : transform.position.z;

            transform.position = pos;
        }
    }
}
