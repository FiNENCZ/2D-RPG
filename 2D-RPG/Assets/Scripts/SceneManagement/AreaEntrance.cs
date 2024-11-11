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

    private void Start()
    {
        if (transitionName == SceneManagement.Instance.SceneTransitionName)
        {
            PlayerController.Instance.transform.position = this.transform.position;
            CameraController.Instance.SetPlayerCameraFollow();
            UIFade.Instance.FadeToClear();


            if (fadePortalAfterEnter)
            {
                UILevelDone.Instance.ShowLevelCompleteNotification();
                //enterPortal.SetActive(false);
            }
        }
    }

}
