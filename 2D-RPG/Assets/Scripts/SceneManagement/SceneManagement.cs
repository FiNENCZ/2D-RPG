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

    //public void Start()
    //{
    //    Debug.Log("Start SceenManagement");
    //    PlayerPrefs.DeleteAll();
    //    PlayerPrefs.Save();
    //}

    private void Start()
    {
        //SaveCurrentEnemyStates();
    }

    public void SaveCurrentEnemyStates()
    {
        // Z�sk� n�zev aktu�ln� sc�ny
        string sceneName = SceneManager.GetActiveScene().name;

        // Vytvo�� seznam EnemyState pro v�echny nep��tele na vrstv� "Enemy"
        List<ObjectState> enemyStates = GetEnemiesStatesOnLayer("Enemy");

        foreach (ObjectState enemy in enemyStates)
        {
            Debug.Log(enemy.name);
        }

        // Ulo�� seznam EnemyState do SceneState pod n�zvem aktu�ln� sc�ny
        sceneState.SaveObjectStates(sceneName, enemyStates);
    }

    public bool ExistsEnemyStatesForScene(string sceneName)
    {
       return sceneState.DoesSceneExist(sceneName);
    }

    // Na�te v�echny objekty na vrstv� "Enemy" a vytvo�� pro n� seznam EnemyState
    private List<ObjectState> GetEnemiesStatesOnLayer(string layerName)
    {
        List<ObjectState> enemyStates = new List<ObjectState>();

        // Z�sk� index vrstvy podle jej�ho jm�na
        int enemyLayer = LayerMask.NameToLayer(layerName);

        // Najde v�echny objekty ve sc�n�
        GameObject[] enemyObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in enemyObjects)
        {

            if (obj.layer == enemyLayer)
            {
                // Z�sk� komponentu EnemyHealth
                EnemyHealth enemyHealth = obj.GetComponent<EnemyHealth>();

                // Pokud objekt m� komponentu EnemyHealth, p�id�me jeho stav
                if (enemyHealth != null)
                {
                    // Vytvo�� nov� EnemyState pro ka�d� objekt
                    ObjectState newEnemyState = new ObjectState
                    {
                        name = enemyHealth.name,               // Pou�ije id z EnemyHealth
                        position = obj.transform.position,  // Pou�ije pozici objektu
                        isDestroyed = false           // Nastav� isDead na z�klad� aktivn�ho stavu objektu
                    };
                    Debug.Log(newEnemyState);
                    enemyStates.Add(newEnemyState);
                }
            }
        }

        return enemyStates;
    }
}
