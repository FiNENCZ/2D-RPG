using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Skill { SwordLifesteal, BowCriticArrow, StaffFree}


[CreateAssetMenu(menuName = "New Skill")]
public class SkillInfo : ScriptableObject
{
    public float skillCooldown;
    public int skillMana;
    public Skill skill;
}
