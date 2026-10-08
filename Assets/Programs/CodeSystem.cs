using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using System.Collections;

public class CodeSystem : MonoBehaviour
{
    public TMP_InputField codeInput;
    public TMP_Text errorText;

    [Header("Google Sheets API")]
    public string googleSheetURL;

    public string nextSceneName = "GameScene";

    void Start()
    {
        codeInput.Select();
        codeInput.ActivateInputField();

        codeInput.onSubmit.AddListener(OnCodeSubmit);

        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }

    void OnCodeSubmit(string text)
    {
        CheckCode();
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

        if (string.IsNullOrEmpty(enteredCode))
        {
            ShowError("Masukkan kode terlebih dahulu!");
            return;
        }

        StartCoroutine(CheckCodeFromGoogleSheets(enteredCode));
    }

    IEnumerator CheckCodeFromGoogleSheets(string code)
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Validating Code...";
        }

        string url =
            googleSheetURL +
            "?code=" +
            UnityWebRequest.EscapeURL(code) +
            "&userName=Player";

        using (UnityWebRequest request =
            UnityWebRequest.Get(url))
        {
            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "Gagal menghubungi Google Sheets: " +
                    request.error
                );

                ShowError(
                    "Gagal terhubung ke server."
                );

                yield break;
            }

            string response = request.downloadHandler.text;

            Debug.Log(
                "RESPON GOOGLE SHEETS: " +
                response
            );

            if (response.Contains(
                "\"success\":true"
            ))
            {
                Debug.Log("KODE BERHASIL DIGUNAKAN!");

                SceneManager.LoadScene(
                    nextSceneName
                );
            }
            else if (response.Contains(
                "Kode sudah digunakan"
                ))
            {
                ShowError(
                    "Code Invalid"
                );
            }
            else if (response.Contains(
                "Kode tidak ditemukan"
                ))
            {
                ShowError(
                    "Code Invalid"
                );
            }
            else
            {
                ShowError(
                    "Kode tidak valid."
                );
            }
        }
    }

    void ShowError(string message)
    {
        if (errorText != null)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = message;
        }

        Debug.Log(message);
    }
}