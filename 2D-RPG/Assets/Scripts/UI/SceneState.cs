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

    // P�id� nebo aktualizuje stav nep��tel pro danou sc�nu
    public void SaveObjectStates(string sceneName, List<ObjectState> enemyStates)
    {
        // Pokud ji� sc�na existuje, aktualizuje ji, jinak ji p�id�
        if (sceneEnemyStates.ContainsKey(sceneName))
        {
            sceneEnemyStates[sceneName] = enemyStates;
        }
        else
        {
            sceneEnemyStates.Add(sceneName, enemyStates);
            Debug.Log("Stava ulo�en");
        }
    }

    // Na�te stav nep��tel pro konkr�tn� sc�nu
    public List<ObjectState> LoadObjectStates(string sceneName)
    {
        if (sceneEnemyStates.TryGetValue(sceneName, out List<ObjectState> enemyStates))
        {
            return enemyStates;
        }
        // Pokud sc�na je�t� nen� v seznamu, vr�t� pr�zdn� seznam
        return new List<ObjectState>();
    }

    // Vy�ist� v�echny stavy nep��tel - m��e� volat p�i resetu v�ech sc�n
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
