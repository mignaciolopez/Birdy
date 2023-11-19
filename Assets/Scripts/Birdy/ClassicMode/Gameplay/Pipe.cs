using System;
using UnityEngine;

namespace Birdy.ClassicMode.Gameplay
{
    public class Pipe : MonoBehaviour
    {
        public static event Action OnPipeHit;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnPipeHit?.Invoke();
        }
    }
}