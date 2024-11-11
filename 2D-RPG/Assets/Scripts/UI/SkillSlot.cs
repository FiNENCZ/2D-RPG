using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillSlot : MonoBehaviour
{
    [SerializeField] private SkillInfo skillInfo;

    private ISkill currentSkill;

    public SkillInfo GetSkillInfo()
    {
        return skillInfo;
    }

    private void Start()
    {
        AttachSkillScript();
    }

    private void AttachSkillScript()
    {
        switch (skillInfo.skill)
        {
            case Skill.SwordLifesteal:
                currentSkill = gameObject.AddComponent<SwordLifesteal>();
                break;
            case Skill.BowCriticArrow:
                currentSkill = gameObject.AddComponent<BowCriticArrow>();
                break;
            case Skill.StaffFree:
                currentSkill = gameObject.AddComponent<StaffFreeze>();
                break;
        }
    }

    public void UseSkill()
    {
        currentSkill.ExecuteSkill();
    }
}
