using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BoardSceneManager : MonoBehaviour
{
    private readonly float delayBeforeLoad = 0.7f;

    public void BackToMenu()
    {
        _ = StartCoroutine(LoadSceneAfterDelay(0));
    }

    private IEnumerator LoadSceneAfterDelay(int indexScene)
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(indexScene);
    }
}
