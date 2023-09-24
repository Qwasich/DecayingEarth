using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DecayingEarth
{
    public class LoaderAsync : MonoBehaviour
    {
        [SerializeField] private string SceneToLoad;

        private void Start()
        {
            if (SceneToLoad == null) return;
            StartCoroutine(LoadNewScene());
        }


        IEnumerator LoadNewScene()
        {
            yield return new WaitForSeconds(1);
            AsyncOperation async = SceneManager.LoadSceneAsync(SceneToLoad);

            while (!async.isDone)
            {
                yield return null;
            }

            Resources.UnloadUnusedAssets();

        }
    }
}
