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


        skillSlot.UseSkill();
    }

    public void SwitchSkillSet(int skillSetNumber)
    {
        switch (skillSetNumber)
        {
            case 0:
                swordSkill.SetActive(true);
                bowSkill.SetActive(false);
                staffSkill.SetActive(false);
                CurrentSkillSet = SkillSet.Sword;
                break;

            case 1:
                swordSkill.SetActive(false);
                bowSkill.SetActive(true);
                staffSkill.SetActive(false);
                CurrentSkillSet = SkillSet.Bow;
                break;

            case 2:
                swordSkill.SetActive(false);
                bowSkill.SetActive(false);
                staffSkill.SetActive(true);
                CurrentSkillSet = SkillSet.Staff;
                break;

            default:
                swordSkill.SetActive(false);
                bowSkill.SetActive(false);
                staffSkill.SetActive(false);
                break;
        }
    }

}
