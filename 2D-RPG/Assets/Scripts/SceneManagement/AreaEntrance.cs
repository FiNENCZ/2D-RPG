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
        // Spust�me ShowLevelCompleteNotification a po�k�me na dokon�en�
        UILevelDone.Instance.ShowLevelCompleteNotification();
        yield return StartCoroutine(UILevelDone.Instance.ShowLevelCompleteRoutine());

        // Pot�, co se dokon�� prvn� korutina, spust�me ShowNewSkillUnlocked
        UINewSkill.Instance.ShowNewSkillUnlocked(skillToUnlock);
    }
}
