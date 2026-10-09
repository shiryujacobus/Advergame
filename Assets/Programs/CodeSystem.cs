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

    public string nextSceneName = "CafeGame";

    private bool isChecking = false;
    private string pendingCode = "";

    void Start()
    {
        DontDestroyOnLoad(gameObject);

        codeInput.Select();
        codeInput.ActivateInputField();

        codeInput.onSubmit.AddListener(OnCodeSubmit);
        codeInput.onValueChanged.AddListener(OnCodeChanged);

        SceneManager.sceneLoaded += OnSceneLoaded;

        if (errorText != null)
        {
            errorText.gameObject.SetActive(false);
        }
    }

    void OnCodeChanged(string value)
    {
        string upperCaseValue = value.ToUpper();

        if (codeInput.text != upperCaseValue)
        {
            codeInput.text = upperCaseValue;
            codeInput.caretPosition = codeInput.text.Length;
        }
    }

    void OnCodeSubmit(string text)
    {
        CheckCode();
    }

    public void CheckCode()
    {
        if (isChecking)
            return;

        string enteredCode = codeInput.text.Trim();

        if (string.IsNullOrEmpty(enteredCode))
        {
            ShowError("Input Code!");
            return;
        }

        StartCoroutine(
            CheckCodeFromGoogleSheets(enteredCode)
        );
    }

    IEnumerator CheckCodeFromGoogleSheets(string code)
    {
        isChecking = true;

        if (errorText != null)
        {
            errorText.gameObject.SetActive(true);
            errorText.text = "Validating Code...";
        }

        string url =
            googleSheetURL +
            "?code=" +
            UnityWebRequest.EscapeURL(code);

        using (UnityWebRequest request =
            UnityWebRequest.Get(url))
        {
            // Kalau terlalu lama, request dianggap timeout.
            // Code BELUM menjadi Used.
            request.timeout = 8;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "GOOGLE SHEETS ERROR: " +
                    request.error
                );

                ShowError(
                    "Gagal terhubung ke server."
                );

                isChecking = false;

                yield break;
            }

            string response =
                request.downloadHandler.text;

            Debug.Log(
                "RESPON VALIDASI: " +
                response
            );

            if (response.Contains(
                "\"success\":true"
            ))
            {
                Debug.Log(
                    "KODE VALID! MENYIAPKAN GAME..."
                );

                // Simpan code sementara.
                pendingCode = code;

                // Masuk ke CafeGame.
                // Code BELUM menjadi Used.
                SceneManager.LoadScene(
                    nextSceneName
                );
            }
            else if (response.Contains(
                "Kode sudah digunakan"
            ))
            {
                ShowError(
                    "Code Invalid!"
                );

                isChecking = false;
            }
            else if (response.Contains(
                "Kode tidak ditemukan"
            ))
            {
                ShowError(
                    "Code Invalid!"
                );

                isChecking = false;
            }
            else
            {
                ShowError(
                    "Code Invalid!"
                );

                isChecking = false;
            }
        }
    }

    void OnSceneLoaded(
        Scene scene,
        LoadSceneMode mode
    )
    {
        if (scene.name != nextSceneName)
            return;

        if (string.IsNullOrEmpty(pendingCode))
            return;

        Debug.Log(
            "CAFEGAME BERHASIL LOADED!"
        );

        StartCoroutine(
            ConfirmCodeUsed(pendingCode)
        );
    }

    IEnumerator ConfirmCodeUsed(string code)
    {
        string url =
            googleSheetURL;

        WWWForm form = new WWWForm();

        form.AddField(
            "code",
            code
        );

        using (UnityWebRequest request =
            UnityWebRequest.Post(url, form))
        {
            request.timeout = 8;

            yield return request.SendWebRequest();

            if (request.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError(
                    "GAGAL CONFIRM CODE: " +
                    request.error
                );

                // Code tetap belum tentu Used.
                // Karena request confirm gagal.
                yield break;
            }

            string response =
                request.downloadHandler.text;

            Debug.Log(
                "RESPON CONFIRM CODE: " +
                response
            );

            if (response.Contains(
                "\"success\":true"
            ))
            {
                Debug.Log(
                    "CODE BERHASIL DITANDAI USED!"
                );

                pendingCode = "";
            }
            else
            {
                Debug.LogError(
                    "CODE GAGAL DI-CONFIRM!"
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

            errorText.ForceMeshUpdate();
        }

        Debug.Log(message);
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}