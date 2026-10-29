using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [SerializeField] private Text statusText;
    [SerializeField] private GameObject instructionsPanel;
    [SerializeField] private float statusDuration = 3f;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(true);
        if (statusText != null)
            statusText.text = "";
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
            ToggleInstructions();
    }

    public void ShowStatus(string message)
    {
        if (statusText == null) return;
        statusText.text = message;
        StopAllCoroutines();
        StartCoroutine(ClearStatus());
    }

    IEnumerator ClearStatus()
    {
        yield return new WaitForSeconds(statusDuration);
        statusText.text = "";
    }

    public void ToggleInstructions()
    {
        if (instructionsPanel != null)
            instructionsPanel.SetActive(!instructionsPanel.activeSelf);
    }

    public void ExitGame()
    {
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
