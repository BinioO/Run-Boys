using UnityEngine;
using System.Collections;
using UnityEditor;

public class MakeScriptableObject  {
    [MenuItem("Assets/Create/ScriptableObject/Enemy/Types")]
    public static void CreateMyAsset()
    {
        EnemyType asset = ScriptableObject.CreateInstance<EnemyType>();

        AssetDatabase.CreateAsset(asset, "Assets/NewEnemyType.asset");
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;
    }
}
