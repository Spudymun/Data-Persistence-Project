using System.Collections;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public float delayBeforeLoad = 0.7f;
    private string userName;
    private ScoreRaw bestScore;

    public void Awake()
    {
        LoadBestScore();
        SetUpBestScore();
    }

    public void LoadBestScore()
    {
        bestScore = LeadBoardManager.Instance.LoadTop();
    }

    private void SetUpBestScore()
    {
        TMP_Text bestScoreText = GameObject.Find("Best Score").GetComponent<TMP_Text>();
        bestScoreText.text =
            bestScore != null
                ? $"Best Score: {bestScore.name}: {bestScore.score}"
                : "Best Score: 0";
    }

    public void StartNew()
    {
        userName = GameObject.Find("Username Input").GetComponent<TMP_InputField>().text;
        _ = StartCoroutine(LoadSceneAfterDelay(1));
    }

    public void OpenLeadBoard()
    {
        _ = StartCoroutine(LoadSceneAfterDelay(2));
    }

    private IEnumerator LoadSceneAfterDelay(int indexScene)
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(indexScene);
        UserDataTransfer.Instance.userName = userName;
    }

    public void Exit()
    {
        _ = StartCoroutine(ExitAfterDelay());
    }

    private IEnumerator ExitAfterDelay()
    {
        // ∆дем указанное врем€
        yield return new WaitForSeconds(delayBeforeLoad);

#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
