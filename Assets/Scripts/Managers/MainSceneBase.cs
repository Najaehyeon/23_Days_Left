using _23DaysLeft.UI;
using System;
using System.Collections;
using UnityEngine;

namespace _23DaysLeft.Managers
{
    public class MainSceneBase : MonoBehaviour
    {
        [SerializeField] private MainSceneUIList uiList;
        [SerializeField] private GameObject playerPrefab;
        [SerializeField] private Transform playerSpawnPoint;

        public Action OnMainSceneInitComplete;
        private static MainSceneBase instance;
        public static MainSceneBase Instance => !instance ? null : instance;
        public TempInputManager tempInputManager;

        public void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            StartCoroutine(Init());
        }

        private IEnumerator Init()
        {
            CreatePool();
            InitUIManager();
            SpawnPlayer();
            
            yield return new WaitForSeconds(1f);
            OnMainSceneInitComplete?.Invoke();
        }

        private void InitUIManager()
        {
            Global.Instance.UIManager.Init(uiList);
        }

        private void CreatePool()
        {
            // Item
            for (int i = 2002; i < 2006; i++)
            {
                var item = Global.Instance.DataLoadManager.Query(i);
                Global.Instance.PoolManager.CreatePool(item.Prefab, 10);
            }

            var leather = Global.Instance.DataLoadManager.Query(2);
            Global.Instance.PoolManager.CreatePool(leather.Prefab, 10);
            
            // Monster
            var creatures = Global.Instance.DataLoadManager.GetCreatureList();
            foreach (var creature in creatures)
            {
                Global.Instance.PoolManager.CreatePool(creature.Prefab, 10);
            }

            var boss = Global.Instance.DataLoadManager.GetBossList();
            foreach (var b in boss)
            {
                Global.Instance.PoolManager.CreatePool(b.Prefab, 1);
            }
            
            // UI
            var uiArr = uiList.UIPrefabs;
            foreach (var ui in uiArr)
            {
                Global.Instance.PoolManager.CreatePool(ui, 10);
            }
        }

        private void SpawnPlayer()
        {
            var player = Instantiate(playerPrefab, playerSpawnPoint.position, Quaternion.identity);
            player.transform.parent = playerSpawnPoint;
            
            var mainCamera = player.transform.Find("CameraContainer/Main Camera");
            if (mainCamera != null)
            {
                if (mainCamera.TryGetComponent(out Camera camera))
                    tempInputManager.sceneCamera = camera;
                else
                    Debug.LogError("Main Camera의 Camera 컴포넌트를 찾을 수 없습니다!");
            }
            else
                Debug.LogError("Main Camera를 찾을 수 없습니다!");
        }
    }
}
