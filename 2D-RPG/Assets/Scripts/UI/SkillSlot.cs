using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillInfo skillInfo;
    [SerializeField] private Image abilityImage;

    private ISkill currentSkill;

    private bool isCooldown = false;

    public SkillInfo GetSkillInfo()
    {
        return skillInfo;
    }

    private void Start()
    {
        AttachSkillScript();
        abilityImage.fillAmount = 0;
    }

    private void AttachSkillScript()
    {
        switch (skillInfo.skill)
        {
            case Skill.SwordStomp:
                currentSkill = gameObject.AddComponent<SwordStomp>();
                break;
            case Skill.BowTripleArrow:
                currentSkill = gameObject.AddComponent<BowTrippleArrow>();
                break;
            case Skill.StaffSlashLaser:
                currentSkill = gameObject.AddComponent<StaffSlashLaser>();
                break;
        }
    }

    public void UseSkill()
    {
        currentSkill.ExecuteSkill(skillInfo.skillCooldown);
        ApplyUICooldown();
    }

    private void ApplyUICooldown()
    {
        if (!isCooldown)
        {
            StartCoroutine(CooldownRoutine());
        }
    }

    private IEnumerator CooldownRoutine()
    {
        isCooldown = true;
        float cooldownTime = skillInfo.skillCooldown;
        abilityImage.fillAmount = 1;

        while (abilityImage.fillAmount > 0)
        {
            // Sni�uje fillAmount podle uplynul�ho �asu.
            abilityImage.fillAmount -= Time.deltaTime / cooldownTime;
            yield return null; // �ek� jeden sn�mek
        }

        abilityImage.fillAmount = 0;
        isCooldown = false;
    }
}
