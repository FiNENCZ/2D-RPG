using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StaffFreeze : MonoBehaviour, ISkill
{
    private Staff staff;

    public float slashDeegre = 80f;

    private void Start()
    {
        staff = FindObjectOfType<Staff>();
    }
    public void ExecuteSkill(float cooldown)
    {
        GameObject newLaser = Instantiate(staff.magicLaser, staff.magicLaserSpawnPoint.position, Quaternion.identity);
        newLaser.GetComponent<MagicLaser>().LaserSlash(staff.weaponInfo.weaponRange, slashDeegre);
    }
}
