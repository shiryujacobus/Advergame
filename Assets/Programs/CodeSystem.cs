using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class CodeSystem : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_Text errorText;

    public string[] validCodes;

    public string nextSceneName = "GameScene";

    void Start()
    {
        codeInput.Select();
        codeInput.ActivateInputField();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            CheckCode();
        }
    }

    public void CheckCode()
    {
        string enteredCode = codeInput.text.Trim();

        foreach (string code in validCodes)
        {
            if (enteredCode == code)
            {
                Debug.Log("KODE BENAR!");

                SceneManager.LoadScene(nextSceneName);
                return;
            }
        }

        Debug.Log("KODE SALAH!");

        if (errorText != null)
        {
            errorText.text = "Invalid Code!";
            errorText.gameObject.SetActive(true);
        }
    }
}