using UnityEngine;

namespace _23DaysLeft.UI
{
    public class MainSceneUIList : MonoBehaviour
    {
        [Header("Canvas")]
        [SerializeField] private Transform screenCanvas;
        [SerializeField] private Transform worldCanvas;

        [Header("UI")]
        [SerializeField] private BossInfoPanel bossInfoPanel;

        [Header("Prefabs")]
        [SerializeField] private GameObject[] uiPrefabs;
        
        public Transform ScreenCanvas => screenCanvas;
        public Transform WorldCanvas => worldCanvas;
        public BossInfoPanel BossInfoPanel => bossInfoPanel;
        public GameObject[] UIPrefabs => uiPrefabs;
    }
}