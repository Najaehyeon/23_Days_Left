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
        [SerializeField] private PlayerConditionPanel playerConditionPanel;
        [SerializeField] private DatePanel datePanel;

        [Header("Prefabs")]
        [SerializeField] private GameObject[] uiPrefabs;
        
        public Transform ScreenCanvas => screenCanvas;
        public Transform WorldCanvas => worldCanvas;
        public BossInfoPanel BossInfoPanel => bossInfoPanel;
        public PlayerConditionPanel PlayerConditionPanel => playerConditionPanel;
        public DatePanel DayPanel => datePanel;
        public GameObject[] UIPrefabs => uiPrefabs;
    }
}