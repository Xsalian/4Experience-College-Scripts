using UnityEngine;

namespace CollegeTD 
{
    public class MainMenuUIManager : MonoBehaviour
    {
        [field: Space, Header("UI References")]
        [field: SerializeField]
        private GameObject MenuUI { get; set; }
        [field: SerializeField]
        private GameObject LevelSelectUI { get; set; }

        public void StartButton ()
        {
            MenuUI.SetActive(false);
            LevelSelectUI.SetActive(true);
        }

        public void ExitButton ()
        {
            Application.Quit();
        }

        public void LoadLevelButton (string sceneName)
        {
            LevelManager.Instance.LoadSceneSingle(sceneName);
        }
    }
}
