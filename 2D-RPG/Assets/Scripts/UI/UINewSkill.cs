using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UINewSkill : Singleton<UINewSkill>
{

    [SerializeField] private Image skillImage;
    [SerializeField] private TextMeshProUGUI skillInfo;
    [SerializeField] private TextMeshProUGUI title;
    [SerializeField] private Image background;

    [SerializeField] private float screenDuration = 3;
    [SerializeField] private float fadeSpeed = 1.5f;
    [SerializeField] private float backgroundAlpha = 0.5f;

    private Sprite stompImage;
    private Sprite trippleArrowImage;
    private Sprite slashLaserImage;

    private string stompText;
    private string trippleArrowText;
    private string slashLaserText;

    private IEnumerator fadeRoutine;

    private void Start()
    {
        // Naèítá obrázek jako Sprite z Resources
        stompImage = Resources.Load<Sprite>("Sprites/Skills/Stomp");
        trippleArrowImage = Resources.Load<Sprite>("Sprites/Skills/CriticArrow");
        slashLaserImage = Resources.Load<Sprite>("Sprites/Skills/Freeze");

        stompText = "Throws off nearby enemies";
        trippleArrowText= "Shoots 3 arrows at each other";
        slashLaserText = "Launches the leser across the board";

        DeactivateAllSkills();

    }

    public void DeactivateAllSkills()
    {
        foreach (Skill skill in System.Enum.GetValues(typeof(Skill)))
        {
            ShowHideAndActivateSkill(skill, false);
        }
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

    public void ShowNewSkillUnlocked(Skill skill) 
    {
        if (SceneManagement.Instance.IsSkillUnlocked(skill))
        {
            return;
        }

        switch (skill)
        {
            case Skill.SwordStomp:
                skillImage.sprite = stompImage;
                skillInfo.text = stompText;
                ActiveInventory.Instance.ToggleActiveSlot(1);
                SceneManagement.Instance.UnlockSkill(skill);
                ShowHideAndActivateSkill(skill, true);
                ActiveInventory.Instance.ToggleActiveSlot(ActiveInventory.Instance.activeSlotIndexNum + 1);
                break;
            case Skill.StaffSlashLaser:
                skillImage.sprite = slashLaserImage;
                skillInfo.text = slashLaserText;
                ActiveInventory.Instance.ToggleActiveSlot(2);
                SceneManagement.Instance.UnlockSkill(skill);
                ShowHideAndActivateSkill(skill, true);
                ActiveInventory.Instance.ToggleActiveSlot(ActiveInventory.Instance.activeSlotIndexNum + 1);
                break;
            case Skill.BowTripleArrow:
                skillImage.sprite = trippleArrowImage;
                skillInfo.text = trippleArrowText;
                ActiveInventory.Instance.ToggleActiveSlot(3);
                SceneManagement.Instance.UnlockSkill(skill);
                ShowHideAndActivateSkill(skill, true);
                ActiveInventory.Instance.ToggleActiveSlot(ActiveInventory.Instance.activeSlotIndexNum + 1);
                break;

        }

        StartCoroutine(ShowNewSkillUnlockedRoutine());
    }

    public void ShowHideAndActivateSkill(Skill skill, bool active)
    {
        // Najde všechny objekty s komponentou SkillSlot
        SkillSlot[] skillSlots = FindObjectsOfType<SkillSlot>();

        foreach (SkillSlot slot in skillSlots)
        {
           
            // Zkontroluje, zda má SkillInfo a zda se shoduje jméno skillu
            if (slot.GetSkillInfo() != null && slot.GetSkillInfo().skill == skill)
            {
                slot.GetSkillInfo().isUnlocked = active;

                foreach (Transform child in slot.transform)
                {
                    child.gameObject.SetActive(active); // nebo child.gameObject.SetActive(false);
                }
            }
        }
    }

    private IEnumerator ShowNewSkillUnlockedRoutine()
    {
        yield return new WaitForSeconds(1);
        FadeToBlack();
        yield return new WaitForSeconds(screenDuration);
        FadeToClear();

    }

    private IEnumerator FadeRoutine(float backgroundTargetAlpha, float textImageTargetAlpha)
    {
        // Zajistíme, že oba objekty budou mít stejný cílový alpha kanál
        bool isFading = true;
        while (isFading)
        {
            // Pøedstavujeme nový alpha kanál pro oba objekty
            float backgroundAlpha = Mathf.MoveTowards(background.color.a, backgroundTargetAlpha, fadeSpeed * Time.deltaTime);
            float textAlpha = Mathf.MoveTowards(skillInfo.color.a, textImageTargetAlpha, fadeSpeed * Time.deltaTime);
            float imageAlpha = Mathf.MoveTowards(skillImage.color.a, textImageTargetAlpha, fadeSpeed * Time.deltaTime);
            float titleAlpha = Mathf.MoveTowards(title.color.a, textImageTargetAlpha, fadeSpeed * Time.deltaTime);

            // Nastavíme nové barvy s upraveným alpha kanálem pro oba objekty
            background.color = new Color(background.color.r, background.color.g, background.color.b, backgroundAlpha);
            skillInfo.color = new Color(skillInfo.color.r, skillInfo.color.g, skillInfo.color.b, textAlpha);
            skillImage.color = new Color(skillImage.color.r, skillImage.color.g, skillImage.color.b, imageAlpha);
            title.color = new Color(title.color.r, title.color.g, title.color.b, imageAlpha);

            // Pokud oba objekty dosáhnou požadované hodnoty alfa, smyèka skonèí
            if (Mathf.Approximately(backgroundAlpha, backgroundTargetAlpha) && Mathf.Approximately(textAlpha, textImageTargetAlpha) &&
                Mathf.Approximately(textAlpha, textImageTargetAlpha) && Mathf.Approximately(titleAlpha, textImageTargetAlpha))
            {
                isFading = false;
            }

            // Èekáme na další snímek
            yield return null;
        }
    }
}
