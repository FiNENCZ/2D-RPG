using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BowCriticArrow : MonoBehaviour, ISkill
{
    [SerializeField] private float shotDelay = 0.1f;

    private Bow bow;
    private bool isReady = true;


    private void Start()
    {
        bow = FindObjectOfType<Bow>();
    }

    public void ExecuteSkill(float cooldown)
    {
        if (bow != null && isReady)
        {
            StartCoroutine(ExecuteSkillRoutine(cooldown));
        }
    }

    private IEnumerator ExecuteSkillRoutine(float cooldown)
    {

        bow.Attack();
        yield return new WaitForSeconds(shotDelay);
        bow.Attack();
        yield return new WaitForSeconds(shotDelay);
        bow.Attack();

        StartCoroutine(SetCooldown(cooldown));
    }

    private IEnumerator SetCooldown(float cooldown)
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }
}
