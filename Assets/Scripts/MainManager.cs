using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public string userName;
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public Text ScoreText;
    public GameObject GameOverText;

    private bool m_Started = false;
    private int m_Points;
    private bool m_GameOver = false;
    private readonly float delayBeforeLoad = 0.7f;

    private ScoreRaw bestScore;
    private Transform backButtonTransform;

    public void TrySaveScore()
    {
        ScoreRaw data = new(userName, m_Points);
        if (LeadBoardManager.Instance != null)
        {
            LeadBoardManager.Instance.AddNewScore(data);
        }
    }

    public void LoadBestScore()
    {
        if (LeadBoardManager.Instance)
        {
            bestScore = LeadBoardManager.Instance.LoadTop();
        }
    }

    private void Awake()
    {
        backButtonTransform = GameObject.Find("Canvas").transform.Find("Back Button");
        InitScoreRow();
        LoadBestScore();
        SetUpBestScore();
    }

    public void BackToMenu()
    {
        _ = StartCoroutine(LoadSceneAfterDelay(0));
    }

    private IEnumerator LoadSceneAfterDelay(int indexScene)
    {
        yield return new WaitForSeconds(delayBeforeLoad);
        SceneManager.LoadScene(indexScene);
    }

    private void SetUpBestScore()
    {
        Text bestScoreText = GameObject.Find("Best Score Text").GetComponent<Text>();
        bestScoreText.text =
            bestScore != null
                ? $"Best Score: {bestScore.name}: {bestScore.score}"
                : "Best Score: 0";
    }

    // Start is called before the first frame update
    private void Start()
    {
        backButtonTransform.gameObject.SetActive(false);
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new[] { 1, 1, 2, 2, 5, 5 };
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new(-1.5f + (step * x), 2.5f + (i * 0.3f), 0);
                Brick brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
    }

    private void InitScoreRow()
    {
        if (UserDataTransfer.Instance)
        {
            userName = UserDataTransfer.Instance.userName;
            ScoreText.text = $"Score : {userName} : 0";
        }
        else
        {
            ScoreText.text = $"Score: 0";
        }
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    private void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {userName} : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;
        GameOverText.SetActive(true);
        backButtonTransform.gameObject.SetActive(true);
        TrySaveScore();
        LoadBestScore();
        SetUpBestScore();
    }
}
