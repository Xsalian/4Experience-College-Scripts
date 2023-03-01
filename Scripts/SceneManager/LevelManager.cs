using UnityEngine;
using UnityEngine.SceneManagement;

namespace CollegeTD
{
    public class LevelManager : SingletonMonoBehaviour<LevelManager>
    {
        [field: Space, Header("First Scene Reference")]
        [field: SerializeField]
        public string FirstSceneName { get; set; }

        private string CurrentSceneName { get; set; }
        private string LastSceneName { get; set; }

        protected override void Awake()
        {
            base.Awake();
            Initialize();
        }

        public void LoadSceneSingle(string sceneName)
        {
            LastSceneName = CurrentSceneName;
            CurrentSceneName = sceneName;
            SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Single);
        }

        public void LoadSceneAdditive(string sceneName)
        {
            LastSceneName = CurrentSceneName;
            CurrentSceneName = sceneName;
            SceneManager.LoadSceneAsync(CurrentSceneName, LoadSceneMode.Additive).completed += OnLoadCompleted;
        }

        public void UnloadScene(string sceneName)
        {
            SceneManager.UnloadSceneAsync(sceneName);
        }

        private void OnLoadCompleted(AsyncOperation async)
        {
            UnloadScene(LastSceneName);
        }

        private void Initialize()
        {
            DontDestroyOnLoad(gameObject);
            LoadSceneSingle(FirstSceneName);
        }
    }
}

