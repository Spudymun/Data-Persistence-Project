using UnityEngine;
using UnityEngine.UI;

public class PressButton : MonoBehaviour
{
    public Animator animator;
    public Button button;
    private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        button.onClick.AddListener(OnButtonClick);
        audioSource = GetComponent<AudioSource>();
    }

    void OnButtonClick()
    {
        animator.SetTrigger("Press Trigger");
        audioSource.PlayOneShot(audioSource.clip);
    }
}
