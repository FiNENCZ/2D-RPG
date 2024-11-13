using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagement : Singleton<SceneManagement>
{
    public string SceneTransitionName { get; private set; }
    [SerializeField] public SceneState sceneState;

    public void SetTransitionName(string sceneTransitionName)
    {
        this.SceneTransitionName = sceneTransitionName;
    }


    private void Start()
    {
        sceneState.ClearAllStates();
    }

    public void SaveCurrentEnemyStates()
    {
        // Získá název aktuální scény
        string sceneName = SceneManager.GetActiveScene().name;

        // Vytvoøí seznam EnemyState pro všechny nepøátele na vrstvì "Enemy"
        List<ObjectState> enemyStates = GetEnemiesStatesOnLayer("Enemy");

        foreach (ObjectState enemy in enemyStates)
        {
            Debug.Log(enemy.name);
        }

        // Uloží seznam EnemyState do SceneState pod názvem aktuální scény
        sceneState.SaveObjectStates(sceneName, enemyStates);
    }

    public bool ExistsEnemyStatesForScene(string sceneName)
    {
       return sceneState.DoesSceneExist(sceneName);
    }

    // Naète všechny objekty na vrstvì "Enemy" a vytvoøí pro nì seznam EnemyState
    private List<ObjectState> GetEnemiesStatesOnLayer(string layerName)
    {
        List<ObjectState> enemyStates = new List<ObjectState>();

        // Získá index vrstvy podle jejího jména
        int enemyLayer = LayerMask.NameToLayer(layerName);

        // Najde všechny objekty ve scénì
        GameObject[] enemyObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in enemyObjects)
        {

            if (obj.layer == enemyLayer)
            {
                // Získá komponentu EnemyHealth
                EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();

                // Pokud objekt má komponentu EnemyHealth, pøidáme jeho stav
                if (enemyHealth != null)
                {
                    // Vytvoøí nový EnemyState pro každý objekt
                    ObjectState newEnemyState = new ObjectState
                    {
                        name = enemyHealth.name,               // Použije id z EnemyHealth
                        position = obj.transform.position,  // Použije pozici objektu
                        isDestroyed = false           // Nastaví isDead na základì aktivního stavu objektu
                    };
                    Debug.Log(newEnemyState);
                    enemyStates.Add(newEnemyState);
                }
            }
        }

        return enemyStates;
    }

    public void UnlockSkill(Skill skill)
    {
        sceneState.UnlockSkill(skill);
    }

    public bool IsSkillUnlocked(Skill skill)
    {
        return sceneState.IsSkillUnlocked(skill);
    }

    public void RestartGame()
    {
        sceneState.ClearAllStates();
        PlayerController.Instance.SetStartingPosition();
        SceneManager.LoadScene("Scene1");
        UINewSkill.Instance.DeactivateAllSkills();

    }
}
