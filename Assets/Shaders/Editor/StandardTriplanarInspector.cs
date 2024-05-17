// Standard shader with triplanar mapping
// https://github.com/keijiro/StandardTriplanar

using UnityEngine;
using UnityEditor;

namespace Shaders.Editor
{
    public class StandardTriplanarInspector : ShaderGUI
    {
    	private static class Styles
    	{
		    public static readonly GUIContent albedo = new("Albedo", "Albedo (RGB)");
		    public static readonly GUIContent normalMap = new("Normal Map", "Normal Map");
		    public static readonly GUIContent occlusion = new("Occlusion", "Occlusion (G)");
    	}
    
    	private bool _initialized;
    
    	public override void OnGUI(MaterialEditor editor, MaterialProperty[] props)
    	{
    		EditorGUI.BeginChangeCheck();
    
    		editor.TexturePropertySingleLine(
    			Styles.albedo, FindProperty("_MainTex", props), FindProperty("_Color", props)
    		);
    
    		editor.ShaderProperty(FindProperty("_Metallic", props), "Metallic");
    		editor.ShaderProperty(FindProperty("_Glossiness", props), "Smoothness");
    
    		var normal = FindProperty("_BumpMap", props);
    		editor.TexturePropertySingleLine(
    			Styles.normalMap, normal,
    			normal.textureValue ? FindProperty("_BumpScale", props) : null
    		);
    
    		var occ = FindProperty("_OcclusionMap", props);
    		editor.TexturePropertySingleLine(
    			Styles.occlusion, occ,
    			occ.textureValue ? FindProperty("_OcclusionStrength", props) : null
    		);
    
    		editor.ShaderProperty(FindProperty("_MapScale", props), "Texture Scale");
    
    		if (EditorGUI.EndChangeCheck() || !_initialized)
    			foreach (Object obj in editor.targets)
			    {
				    var material = (Material)obj;
				    SetMaterialKeywords(material);
			    }

		    _initialized = true;
    	}
    
    	private static void SetMaterialKeywords(Material material)
    	{
    		SetKeyword(material, "_NORMALMAP", material.GetTexture("_BumpMap"));
    		SetKeyword(material, "_OCCLUSIONMAP", material.GetTexture("_OcclusionMap"));
    	}
    
    	private static void SetKeyword(Material m, string keyword, bool state)
    	{
    		if (state)
    			m.EnableKeyword(keyword);
    		else
    			m.DisableKeyword(keyword);
    	}
    }
}