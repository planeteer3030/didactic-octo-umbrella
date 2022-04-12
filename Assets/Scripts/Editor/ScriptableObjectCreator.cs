using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor tool for quick ScriptableObject creation.
/// </summary>
public static class ScriptableObjectCreator
{
	[MenuItem("Assets/Create/Scriptable Object", false, 0)]
	public static void CreateScriptableObject()
	{
		var selection = Selection.activeObject;
		var assetPath = AssetDatabase.GetAssetPath(selection.GetInstanceID());
		var path = $"{System.IO.Directory.GetParent(assetPath)}/{selection.name}.asset";
		var instance = ScriptableObject.CreateInstance(selection.name);
		if (instance != null)
			ProjectWindowUtil.CreateAsset(instance, path);
	}

	[MenuItem("Assets/Create/Scriptable Object", true)]
	public static bool CreateScriptableObjectValidate()
	{
		return Selection.activeObject is MonoScript monoScript && IsScriptableObject(monoScript.GetClass());
	}

	private static bool IsScriptableObject(System.Type type) => typeof(ScriptableObject).IsAssignableFrom(type);
}