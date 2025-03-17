using _23DaysLeft.UI;
using System;
using UnityEngine;

namespace _23DaysLeft.Managers
{
    public class MainSceneBase : MonoBehaviour
    {
        [SerializeField] private MainSceneUIList uiList;

        public Action OnMainSceneInitComplete;
        private static MainSceneBase instance;
        public static MainSceneBase Instance => !instance ? null : instance;

        /// 씬이 로드되었을 때 할 일
        // pool 만들기
        // uiManager 초기화
        // 플레이어 생성

        public void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            CreatePool();
            InitUIManager();
            OnMainSceneInitComplete?.Invoke();
        }

        private void InitUIManager()
        {
            UIManager.Instance.Init(uiList);
        }

        private void CreatePool()
        {
            var creatures = Global.Instance.DataLoadManager.GetCreatureList();
            foreach (var creature in creatures)
            {
                PoolManager.Instance.CreatePool(creature.Prefab, 10);
            }

            var boss = Global.Instance.DataLoadManager.GetBossList();
            foreach (var b in boss)
            {
                PoolManager.Instance.CreatePool(b.Prefab, 1);
            }
        }
    }
}
