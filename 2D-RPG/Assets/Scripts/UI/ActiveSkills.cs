using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using Unity.VisualScripting;
using UnityEngine;

public class ActiveSkills : Singleton<ActiveSkills>
{
    [SerializeField] private GameObject swordSkill;
    [SerializeField] private GameObject bowSkill;
    [SerializeField] private GameObject staffSkill;

    private PlayerControls playerControls;


    public enum SkillSet
    {
        Sword = 0,
        Bow = 1,
        Staff = 2
    }

    public SkillSet CurrentSkillSet { get; private set; }

    protected override void Awake()
    {
        base.Awake();

        playerControls = new PlayerControls();
        CurrentSkillSet = SkillSet.Sword;
    }

    private void Start()
    {
        playerControls.Skills.Keyboard.performed += ctx => TriggerSkill((int)ctx.ReadValue<float>());
    }

    public void OnEnable()
    {
        playerControls.Enable();
    }

    private void TriggerSkill(int numValue)
    {
        Transform childTransform = transform.GetChild((int)CurrentSkillSet).GetChild(numValue);
        SkillSlot skillSlot = childTransform.GetComponentInChildren<SkillSlot>();
        SkillInfo skillInfo = skillSlot.GetSkillInfo();

        if (skillInfo != null && skillInfo.isUnlocked) 
        {
            skillSlot.UseSkill();
        }
    }

    public void SwitchSkillSet(int skillSetNumber)
    {
        switch (skillSetNumber)
        {
            case 0:
                swordSkill.GetComponent<CanvasGroup>().alpha = 1;
                bowSkill.GetComponent<CanvasGroup>().alpha = 0;
                staffSkill.GetComponent<CanvasGroup>().alpha = 0;
                CurrentSkillSet = SkillSet.Sword;
                break;
            case 1:
                swordSkill.GetComponent<CanvasGroup>().alpha = 0;
                bowSkill.GetComponent<CanvasGroup>().alpha = 0;
                staffSkill.GetComponent<CanvasGroup>().alpha = 1;
                CurrentSkillSet = SkillSet.Staff;
                break;
            case 2:
                swordSkill.GetComponent<CanvasGroup>().alpha = 0;
                bowSkill.GetComponent<CanvasGroup>().alpha = 1;
                staffSkill.GetComponent<CanvasGroup>().alpha = 0;
                CurrentSkillSet = SkillSet.Bow;
                break;
            default:
                swordSkill.GetComponent<CanvasGroup>().alpha = 0;
                bowSkill.GetComponent<CanvasGroup>().alpha = 0;
                staffSkill.GetComponent<CanvasGroup>().alpha = 0;
                break;
        }
    }

}
