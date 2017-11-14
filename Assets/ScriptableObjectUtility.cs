using UnityEngine;
using UnityEditor;
using System.IO;


public class ScriptableObjectUtility : MonoBehaviour {

    public static T CreateAsset<T>() where T : ScriptableObject
    {
        T asset = ScriptableObject.CreateInstance<T>();

        if (!AssetDatabase.IsValidFolder(("Assets/NewObjects")))
            AssetDatabase.CreateFolder("Assets", "NewObjects");

        string assetPathAndName = AssetDatabase.GenerateUniqueAssetPath("Assets/NewObjects/" + typeof(T).ToString() + ".asset");

        AssetDatabase.CreateAsset(asset, assetPathAndName);

        AssetDatabase.SaveAssets();

        AssetDatabase.Refresh();

        EditorUtility.FocusProjectWindow();

        Selection.activeObject = asset;

        return asset;
    }
}
