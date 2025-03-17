using _23DaysLeft.Utils;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _23DaysLeft.Managers
{
    public class SceneLoader : MonoBehaviour
    {
        private const float fakeMinDuration = 2f;
        private const float fakeMaxDuration = 3f;

        public void LoadScene(SceneType loadScene)
        {
            StartCoroutine(LoadSceneCoroutine(loadScene));
        }
        
        // 씬을 로드하는 코루틴
        private IEnumerator LoadSceneCoroutine(SceneType loadScene)
        {
            // 로딩 씬을 먼저 로드
            yield return SceneManager.LoadSceneAsync(SceneType.Loading.GetName(), LoadSceneMode.Additive);
            Global.Instance.UIManager.OnChangeLoadingProgress?.Invoke(0);
            
            // 로드 할 씬이 로드되는 동안 대기
            AsyncOperation operation = SceneManager.LoadSceneAsync(loadScene.GetName(), LoadSceneMode.Additive);
            if (operation == null)
            {
                Debug.LogError("SceneLoader.LoadSceneAsync: operation is null");
                yield break;
            }

            operation.allowSceneActivation = false;

            // 최소 로딩 시간을 보장하기 위해 가짜 로딩 시간을 설정
            float minDuration = Random.Range(fakeMinDuration, fakeMaxDuration);
            float fakeLoadTime = 0f;

            // 씬이 90% 로드될 때까지 로딩바를 채움
            var loadRatio = 0f;
            while (loadRatio < 0.9f)
            {
                fakeLoadTime += Time.deltaTime;
                var fakeLoadRatio = fakeLoadTime / minDuration;

                loadRatio = Mathf.Min(operation.progress, fakeLoadRatio - 0.1f);
                Global.Instance.UIManager.OnChangeLoadingProgress?.Invoke(loadRatio);

                yield return null;
            }
            
            // 로드 씬 활성화
            SceneManager.UnloadSceneAsync(SceneType.Title.GetName());
            operation.allowSceneActivation = true;
            while (!operation.isDone) yield return null;

            // 로드 씬 초기화 완료까지 대기
            yield return InitCoroutine(loadScene);
            
            // 로딩바를 100%로 채움
            while (loadRatio < 1f)
            {
                loadRatio += Time.deltaTime;
                Global.Instance.UIManager.OnChangeLoadingProgress?.Invoke(loadRatio);

                yield return null;
            }
            
            // 로딩 씬을 언로드
            SceneManager.UnloadSceneAsync(SceneType.Loading.GetName());
        }

        private IEnumerator InitCoroutine(SceneType loadScene)
        {
            var isInitComplete = false;
            switch (loadScene)
            {
                case SceneType.Main:
                    MainSceneBase.Instance.OnMainSceneInitComplete += () => isInitComplete = true;
                    break;
                default:
                    isInitComplete = true;
                    break;
            }
            
            while (!isInitComplete) yield return null;
        }
    }

    public enum SceneType
    {
        Title,
        Loading,
        Main,
    }
}