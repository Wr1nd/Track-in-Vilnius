using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class ExtureAssigner : MonoBehaviour
{
    [Header("Drag Folder Path (Relative to Assets)")]
    public string textureFolderPath = "Assets/New Folder/textures";

    [Header("Materials to Update")]
    public string materialFolderPath = "Assets/New Folder/mater";

    void Start()
    {
        //say that it started in console
        Debug.Log("Started");
        
        string[] textureGUIDs = AssetDatabase.FindAssets("t:Texture2D", new[] { textureFolderPath });
        string[] materialGUIDs = AssetDatabase.FindAssets("t:Material", new[] { materialFolderPath });
        
        if (textureGUIDs.Length == 0)
        {
            Debug.LogWarning($"No textures found in folder: {textureFolderPath}");
            return;
        }
        if (materialGUIDs.Length == 0)
        {
            Debug.LogWarning($"No materials found in folder: {materialFolderPath}");
            return;
        }

        // Load textures and materials into arrays
        Texture2D[] textures = new Texture2D[textureGUIDs.Length];
        Material[] materials = new Material[materialGUIDs.Length];

        for (int i = 0; i < textureGUIDs.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(textureGUIDs[i]);
            textures[i] = AssetDatabase.LoadAssetAtPath<Texture2D>(path);
        }

        for (int i = 0; i < materialGUIDs.Length; i++)
        {
            string path = AssetDatabase.GUIDToAssetPath(materialGUIDs[i]);
            materials[i] = AssetDatabase.LoadAssetAtPath<Material>(path);
        }
       
        
        //lenght
        Debug.Log($"Textures: {textures.Length} Materials: {materials.Length}");
        
        
        if (textures.Length == 0)
        {
            Debug.LogWarning($"No textures found in folder: {textureFolderPath}");
            return;
        }

        foreach (Material mat in materials)
        {
            foreach (Texture2D tex in textures)
            {
                //remove the (Image_) from textures and (Material_) from materials
                tex.name = tex.name.Replace("Image_", "");
                mat.name = mat.name.Replace("Material_", "");
                //report names to console
                Debug.Log($"Texture: {tex.name} Material: {mat.name}");
                
                
                
                
                // Assign texture to material if names match
                if (tex.name == mat.name)
                {
                    
                    mat.mainTexture = tex;
                    Debug.Log($"Assigned texture {tex.name} to material {mat.name}");
                    //if assigned, remove the texture and material from the list
                    ArrayUtility.Remove(ref textures, tex);
                    ArrayUtility.Remove(ref materials, mat);
                    
                    
                    //add the material and textures names back
                    tex.name = "Image_" + tex.name;
                    mat.name = "Material_" + mat.name;
                    
                    break;
                }
                
                
            }
        }
    }
}
