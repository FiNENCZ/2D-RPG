using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UILevelDone : Singleton<UILevelDone>
{


    [SerializeField] private GameObject levelDoneContainer;
    [SerializeField] private string levelDoneTextContent = "Level 1 Completed";
    [SerializeField] private float screenDuration = 3;
    [SerializeField] private float fadeSpeed = 1.5f;
    [SerializeField] private float backgroundAlpha = 0.5f;


    private TextMeshProUGUI levelDoneText;
    private Image levelDoneBackground;
    private CanvasGroup levelDoneContainerCanvasGroup;

    private IEnumerator fadeRoutine;

    // Start is called before the first frame update
    void Start()
    {
        levelDoneText = levelDoneContainer.GetComponentInChildren<TextMeshProUGUI>();
        levelDoneBackground = levelDoneContainer.GetComponentInChildren<Image>();

        levelDoneContainerCanvasGroup = levelDoneContainer.GetComponent<CanvasGroup>();
    }

    public void FadeToBlack()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(backgroundAlpha, 1);
        StartCoroutine(fadeRoutine);
    }

    public void FadeToClear()
    {
        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = FadeRoutine(0, 0);
        StartCoroutine(fadeRoutine);
    }

    public void ShowLevelCompleteNotification()
    {
        StartCoroutine(ShowLevelCompleteRoutine());
    }

    public IEnumerator ShowLevelCompleteRoutine()
    {
        yield return new WaitForSeconds(1);
        FadeToBlack();
        yield return new WaitForSeconds(screenDuration);
        FadeToClear();

    }

    private IEnumerator FadeRoutine(float backgroundTargetAlpha, float textTargetAlpha)
    {
        // Zajistíme, že oba objekty budou mít stejný cílový alpha kanál
        bool isFading = true;
        while (isFading)
        {
            // Pøedstavujeme nový alpha kanál pro oba objekty
            float backgroundAlpha = Mathf.MoveTowards(levelDoneBackground.color.a, backgroundTargetAlpha, fadeSpeed * Time.deltaTime);
            float textAlpha = Mathf.MoveTowards(levelDoneText.color.a, textTargetAlpha, fadeSpeed * Time.deltaTime);

            // Nastavíme nové barvy s upraveným alpha kanálem pro oba objekty
            levelDoneBackground.color = new Color(levelDoneBackground.color.r, levelDoneBackground.color.g, levelDoneBackground.color.b, backgroundAlpha);
            levelDoneText.color = new Color(levelDoneText.color.r, levelDoneText.color.g, levelDoneText.color.b, textAlpha);

            // Pokud oba objekty dosáhnou požadované hodnoty alfa, smyèka skonèí
            if (Mathf.Approximately(backgroundAlpha, backgroundTargetAlpha) && Mathf.Approximately(textAlpha, textTargetAlpha))
            {
                isFading = false;
            }

            // Èekáme na další snímek
            yield return null;
        }
    }
}
