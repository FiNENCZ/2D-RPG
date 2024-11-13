using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AreaEntrance : MonoBehaviour
{
    [SerializeField] private string transitionName;
    [SerializeField] private bool fadePortalAfterEnter = false;
    [SerializeField] private GameObject enterPortal;
    [SerializeField] private bool isNewSkillUnlocked = false;
    [SerializeField] private Skill skillToUnlock;

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();

            if (fadePortalAfterEnter && isNewSkillUnlocked)
            {
                ShowLevelCompletionAndSkillUnlocked(skillToUnlock);
            }
            else if (fadePortalAfterEnter)
            {
                UILevelDone.Instance.ShowLevelCompleteNotification();

            } else if (isNewSkillUnlocked)
            {
                UINewSkill.Instance.ShowNewSkillUnlocked(skillToUnlock);
            }
        }
    }

    public void ShowLevelCompletionAndSkillUnlocked(Skill skillToUnlock)
    {
        StartCoroutine(LevelCompletionAndSkillRoutine(skillToUnlock));
    }

    private IEnumerator LevelCompletionAndSkillRoutine(Skill skillToUnlock)
    {
        // Spustíme ShowLevelCompleteNotification a poèkáme na dokonèení
        UILevelDone.Instance.ShowLevelCompleteNotification();
        yield return StartCoroutine(UILevelDone.Instance.ShowLevelCompleteRoutine());

        // Poté, co se dokonèí první korutina, spustíme ShowNewSkillUnlocked
        UINewSkill.Instance.ShowNewSkillUnlocked(skillToUnlock);
    }
}
