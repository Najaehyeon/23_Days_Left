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
        
        public Transform ScreenCanvas => screenCanvas;
        public Transform WorldCanvas => worldCanvas;
        public BossInfoPanel BossInfoPanel => bossInfoPanel;
    }
}