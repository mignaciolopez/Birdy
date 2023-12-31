using UnityEngine;

namespace Birdy.Shared.Management
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance {  get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            DontDestroyOnLoad(gameObject);

        }
    }
}