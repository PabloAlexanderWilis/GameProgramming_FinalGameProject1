using UnityEngine;
using System.Collections.Generic;

public class UniversalSaveManager : MonoBehaviour
{
    [System.Serializable]
    public class GameState
    {
        public List<GameObjectState> objectStates = new List<GameObjectState>();
    }

    [System.Serializable]
    public class GameObjectState
    {
        public string guid; // Unique identifier for each object
        public Vector3 position;
        public Vector3 scale;
        public Quaternion rotation;
        public bool activeSelf;
        public string tag;
        public string layer;
    }

    private static GameState savedState = new GameState();
    private static Dictionary<string, string> objectGuidMap = new Dictionary<string, string>();

    [ContextMenu("Save Current State")]
    public static void SaveCurrentState()
    {
        savedState.objectStates.Clear();
        objectGuidMap.Clear();

        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.CompareTag("MainCamera") || obj.CompareTag("Untagged") ||
                obj.layer == 5) 
                continue;

            string guid = obj.GetInstanceID().ToString(); // auto-generated unique ID
            objectGuidMap[obj.name] = guid;

            var state = new GameObjectState
            {
                guid = guid,
                position = obj.transform.position,
                scale = obj.transform.localScale,
                rotation = obj.transform.rotation,
                activeSelf = obj.activeSelf,
                tag = obj.tag,
                layer = LayerMask.LayerToName(obj.layer)
            };

            savedState.objectStates.Add(state);
        }

        Debug.Log($"Saved state of {savedState.objectStates.Count} objects");
    }

    [ContextMenu("Load Saved State")]
    public static void LoadSavedState()
    {
        foreach (GameObjectState state in savedState.objectStates)
        {
            GameObject obj = GameObject.Find(GetObjectNameFromGuid(state.guid));

            if (obj != null)
            {
                obj.transform.position = state.position;
                obj.transform.localScale = state.scale;
                obj.transform.rotation = state.rotation;
                obj.SetActive(state.activeSelf);

                obj.tag = state.tag;
                obj.layer = LayerMask.NameToLayer(state.layer);
            }
        }

        Debug.Log($"Loaded state of {savedState.objectStates.Count} objects");
    }

    private static string GetObjectNameFromGuid(string guid)
    {
        foreach (var pair in objectGuidMap)
        {
            if (pair.Value == guid)
                return pair.Key;
        }
        return null;
    }

    public static void AutoSaveState()
    {
        SaveCurrentState();
    }

    public static void AutoLoadState()
    {
        LoadSavedState();
    }

    public static bool IsEmpty()
    {
        return savedState.objectStates.Count == 0;
    }
}