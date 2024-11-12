using System.Collections;
using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

public class StaffSlashLaser : MonoBehaviour, ISkill
{
    public float slashDeegre = 80f;

    private Staff staff;
    private bool isReady = true;


    private void Start()
    {
        staff = FindObjectOfType<Staff>();
    }
    public void ExecuteSkill(float cooldown)
    {
        staff = FindObjectOfType<Staff>();
        if (staff != null && isReady)
        {
            GameObject newLaser = Instantiate(staff.magicLaser, staff.magicLaserSpawnPoint.position, Quaternion.identity);
            newLaser.GetComponent<MagicLaser>().LaserSlash(staff.weaponInfo.weaponRange, slashDeegre);

            Debug.Log("Utok provede");
        }

        StartCoroutine(SetCooldown(cooldown));
    }

    private IEnumerator SetCooldown(float cooldown)
    {
        isReady = false;
        yield return new WaitForSeconds(cooldown);
        isReady = true;
    }

}
