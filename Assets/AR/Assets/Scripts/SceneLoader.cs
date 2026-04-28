using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] GameObject loading;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
    public void OnClickLoadScene()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        loading.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync("ARScene");

        while (!operation.isDone)
        {
            yield return null;
        }

        loading.SetActive(false); 
    }
}