using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjectStateManager
{
    public static void SaveDestroyToSceneState(string sceneName, string objectName, Vector3 position)
    {
        // Create new record, if doesn't exist for this scene
        List<ObjectState> objectStates;
        if (!SceneManagement.Instance.sceneState.TryGetObjectStates(sceneName, out objectStates))
        {
            objectStates = new List<ObjectState>();
            SceneManagement.Instance.sceneState.SaveObjectStates(sceneName, objectStates);
        }

        // Find or create new objectState
        ObjectState objectState = objectStates.Find(e => e.name == objectName);
        if (objectState == null)
        {
            objectState = new ObjectState { name = objectName, position = position, isDestroyed = true };
            objectStates.Add(objectState);
        }
        else
        {
            objectState.isDestroyed = true;
            objectState.position = position;
        }

        SceneManagement.Instance.sceneState.SaveObjectStates(sceneName, objectStates);
    }
}
