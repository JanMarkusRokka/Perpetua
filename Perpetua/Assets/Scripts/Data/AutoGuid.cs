using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
// Inspired by SharonL75 https://forum.unity.com/threads/how-can-i-keep-the-objects-in-scene-instance-ids-after-loading-the-scene-again.984077/
public class AutoGuid : MonoBehaviour
{
    [MenuItem("GameObject/Generate Guid", false)]
    private static void GenerateGuid()
    {
        foreach(GameObject obj in Selection.objects)
        {
            if (!obj.GetComponent<GuidGenerator>())
            obj.AddComponent<GuidGenerator>();
            obj.GetComponent<GuidGenerator>().GenGuid();
            obj.name = obj.GetComponent<GuidGenerator>().guidString;
        }
        EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
    }
}
