using UnityEngine;

public class UserDataTransfer : MonoBehaviour
{
    public static UserDataTransfer Instance { get; private set; }
    public string userName;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
