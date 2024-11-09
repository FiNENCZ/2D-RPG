using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelDone : MonoBehaviour
{
    [SerializeField] public GameObject areaExit;

    private int enemyCount;
    private string enemyLayer = "Enemy";

    private void Awake()
    {



        //if (!SceneManagement.Instance.ExistsEnemyStatesForScene(SceneManager.GetActiveScene().name))
        //{
        //    //SceneManagement.Instance.SaveCurrentEnemyStates();
        //}

        LoadEnemies();
        enemyCount = GetEnemyCount();
    }

    private void Start()
    {
       
    }

    private void LoadEnemies()
    {
        foreach (ObjectState enemyState in SceneManagement.Instance.sceneState.LoadObjectStates(SceneManager.GetActiveScene().name))
        {
            if (enemyState.isDestroyed)
            {
                GameObject enemy = GameObject.Find(enemyState.name);
                Debug.Log(enemy);
                if (enemy != null)
                {
                    Destroy(enemy); 
                }
            }
            else
            {
                // Update saved position
                GameObject enemy = GameObject.Find(enemyState.name);
                if (enemy != null)
                {
                    enemy.transform.position = enemyState.position;
                }
            }
        }
    }


    public void EnemyDied()
    {
        enemyCount--;

        if (enemyCount <= 0)
        {
            LevelCompleted();
        }
    }

    private int GetEnemyCount()
    {
        int layer = LayerMask.NameToLayer(enemyLayer);
        GameObject[] allObjects = FindObjectsOfType<GameObject>();
        int count = 0;

        foreach (GameObject obj in allObjects)
        {
            
            if (obj.layer == layer)
            {
                count++;
            }
        }

        return count;
    }

    private void DeleteAllEnemies()
    {
        int layer = LayerMask.NameToLayer(enemyLayer);
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.layer == layer)
            {
                Object.Destroy(obj);
            }
        }

    }

    private void LevelCompleted()
    {
        if (areaExit != null)
        {
            areaExit.SetActive(true);
        }
    }
}
