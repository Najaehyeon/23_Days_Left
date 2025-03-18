using _23DaysLeft.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace _23DaysLeft.UI
{
    public class TestButton : MonoBehaviour
    {
        [SerializeField] private GameObject eventSystem;
        [SerializeField] private Button startBtn;

        private void Start()
        {
            startBtn.onClick.AddListener(StartGame);
        }

        private void StartGame()
        {
            eventSystem.SetActive(false);
            Global.Instance.SceneLoader.LoadScene(SceneType.Main);
        }
    }
}