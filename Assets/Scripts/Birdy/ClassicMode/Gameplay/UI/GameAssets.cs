using UnityEngine;

namespace Birdy.ClassicMode.Gameplay.UI
{
    public class GameAssets : MonoBehaviour
    {
        #region Singleton
        public static GameAssets Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
                Instance = this;

            DontDestroyOnLoad(gameObject);
        }
        #endregion //Singleton

        #region Assets
        public GameObject PipeBody;
        public GameObject PipeHead;
        public GameObject PipeGap;
        public GameObject Background;
        public GameObject Ground;
        public GameObject Spawner;
        public GameObject UnSpawner;
        #endregion //Assets
    }
}