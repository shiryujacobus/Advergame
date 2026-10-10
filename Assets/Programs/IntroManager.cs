
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class IntroManager : MonoBehaviour
{
    [Header("Logos")]
    public CanvasGroup studioLogo;
    public CanvasGroup collaboratorLogo;

    [Header("Credit Texts")]
    public CanvasGroup presentedByText;
    public CanvasGroup collaborationText;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float displayDuration = 2f;

    [Header("Scene")]
    public string mainMenuSceneName = "MainMenu";

    private void Start()
    {
        StartCoroutine(PlayIntro());
    }

    private IEnumerator PlayIntro()
    {
        studioLogo.alpha = 0f;
        collaboratorLogo.alpha = 0f;
        presentedByText.alpha = 0f;
        collaborationText.alpha = 0f;

        yield return ShowCredit(presentedByText, studioLogo);
        yield return ShowCredit(collaborationText, collaboratorLogo);

        SceneManager.LoadScene(mainMenuSceneName);
    }

    private IEnumerator ShowCredit(
        CanvasGroup creditText,
        CanvasGroup logo)
    {
        creditText.gameObject.SetActive(true);
        logo.gameObject.SetActive(true);

        creditText.alpha = 0f;
        logo.alpha = 0f;

        yield return Fade(creditText, 0f, 1f);
        yield return Fade(logo, 0f, 1f);

        yield return new WaitForSeconds(displayDuration);

        yield return Fade(logo, 1f, 0f);
        yield return Fade(creditText, 1f, 0f);
    }

    private IEnumerator Fade(
        CanvasGroup target,
        float start,
        float end)
    {
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;

            target.alpha = Mathf.Lerp(
                start,
                end,
                Mathf.Clamp01(elapsed / fadeDuration)
            );

            yield return null;
        }

        target.alpha = end;
    }
}
