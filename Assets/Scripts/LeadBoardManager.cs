using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeadBoardManager : MonoBehaviour
{
    public static LeadBoardManager Instance { get; private set; }

    public TMP_Text scoreEntryPrefab;
    private Transform contentTransform;

    private string filePath;
    private List<ScoreRaw> highScores = new();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        filePath = Path.Combine(Application.persistentDataPath, "highscores.json");
    }

    public void Initialize()
    {
        InitLogic();
    }

    private void Start()
    {
        InitLogic();
    }

    private void InitLogic()
    {
        LoadScores();
        LoadLeadBoardObject();
    }

    private void LoadLeadBoardObject()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int sceneIndex = currentScene.buildIndex;
        if (sceneIndex == 2)
        {
            contentTransform = GameObject
                .Find("Scroll View")
                .GetComponent<ScrollRect>()
                .content.transform;

            DisplayScores();
        }
    }

    private void DisplayScores()
    {
        foreach (Transform child in contentTransform)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < highScores.Count; i++)
        {
            ScoreRaw score = highScores[i];
            TMP_Text entry = Instantiate(scoreEntryPrefab, contentTransform);
            int rank = i + 1;
            entry.text = $"{rank}. {score.name}: {score.score}";
        }
    }

    public void AddNewScore(ScoreRaw raw)
    {
        highScores.Add(raw);

        highScores.Sort((x, y) => y.score.CompareTo(x.score));

        if (highScores.Count > 10)
        {
            highScores.RemoveAt(highScores.Count - 1);
        }

        SaveScores();
    }

    private void SaveScores()
    {
        string json = JsonUtility.ToJson(new SerializableList<ScoreRaw>(highScores), true);
        File.WriteAllText(filePath, json);
    }

    private void LoadScores()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            SerializableList<ScoreRaw> loadedScores;
            loadedScores = JsonUtility.FromJson<SerializableList<ScoreRaw>>(json);
            highScores = loadedScores.list;
        }
    }

    public ScoreRaw LoadTop()
    {
        LoadScores();
        return highScores.Count > 0 ? highScores[0] : null;
    }
}

[System.Serializable]
public class ScoreRaw
{
    public string name;
    public int score;

    public ScoreRaw(string playerName, int playerScore)
    {
        name = playerName;
        score = playerScore;
    }
}

[System.Serializable]
public class SerializableList<T>
{
    public List<T> list;

    public SerializableList(List<T> list)
    {
        this.list = list;
    }
}
