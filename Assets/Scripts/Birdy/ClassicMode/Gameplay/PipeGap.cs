using System;
using UnityEngine;

namespace Birdy.ClassicMode.Gameplay
{
    public class PipeGap : MonoBehaviour
    {
        public static event Action<int> OnPipePassed;

        private void OnTriggerExit2D(Collider2D collider)
        {
            OnPipePassed?.Invoke(1);
        }
    }
}