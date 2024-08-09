using TMPro;
using UnityEngine;

public class BoardSceneInit : MonoBehaviour
{
    public TMP_Text scoreEntryPrefab;

    private void Start()
    {
        if (LeadBoardManager.Instance == null)
        {
            GameObject lbManagerObj = new("LeadBoardManager");

            LeadBoardManager lbManager = lbManagerObj.AddComponent<LeadBoardManager>();
            lbManager.scoreEntryPrefab = scoreEntryPrefab;
        }
        else
        {
            LeadBoardManager.Instance.Initialize();
        }
    }
}
