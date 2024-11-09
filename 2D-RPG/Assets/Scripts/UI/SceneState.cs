using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class ObjectState
{
    public string name;
    public Vector3 position;
    public bool isDestroyed;
}

[CreateAssetMenu(menuName = "Scene State")]
public class SceneState : ScriptableObject
{
    public Dictionary<string, List<ObjectState>> sceneEnemyStates = new Dictionary<string, List<ObjectState>>();

    // Pøidá nebo aktualizuje stav nepøátel pro danou scénu
    public void SaveObjectStates(string sceneName, List<ObjectState> enemyStates)
    {
        // Pokud již scéna existuje, aktualizuje ji, jinak ji pøidá
        if (sceneEnemyStates.ContainsKey(sceneName))
        {
            sceneEnemyStates[sceneName] = enemyStates;
        }
        else
        {
            sceneEnemyStates.Add(sceneName, enemyStates);
            Debug.Log("Stava uložen");
        }
    }

    // Naète stav nepøátel pro konkrétní scénu
    public List<ObjectState> LoadObjectStates(string sceneName)
    {
        if (sceneEnemyStates.TryGetValue(sceneName, out List<ObjectState> enemyStates))
        {
            return enemyStates;
        }
        // Pokud scéna ještì není v seznamu, vrátí prázdný seznam
        return new List<ObjectState>();
    }

    // Vyèistí všechny stavy nepøátel - mùžeš volat pøi resetu všech scén
    public void ClearAllObjectStates()
    {
        sceneEnemyStates.Clear();
    }

    public bool TryGetObjectStates(string sceneName, out List<ObjectState> enemyStates)
    {
        return sceneEnemyStates.TryGetValue(sceneName, out enemyStates);
    }

    public bool DoesSceneExist(string sceneName)
    {
        return sceneEnemyStates.ContainsKey(sceneName);
    }
}
