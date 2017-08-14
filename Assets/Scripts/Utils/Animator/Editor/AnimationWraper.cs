using System.Collections;
using UnityEngine;
using UnityEditor;
using System.IO;
using System;

public class AnimationWraper
{
    #if UNITY_EDITOR
        [MenuItem("Assets/Wrap Into Animation")]
        private static void WrapIntoAnimation()
        {
            var selected = Selection.GetFiltered(typeof(Texture2D), SelectionMode.Assets);
            Array.Sort(selected, _sortFilesByName());

            var so = ScriptableObject.CreateInstance<AnimationFile>();
            string path = AssetDatabase.GetAssetPath (Selection.activeObject);
            if (path == "")
                return;

            Sprite[] textureArray = new Sprite[selected.Length];
            int i = 0;

            foreach (var gameObject in selected)
            {    
                if (gameObject.GetType() != typeof(Texture2D))
                    continue;
                string assetPath = AssetDatabase.GetAssetPath (gameObject);
                textureArray[(i++)] = AssetDatabase.LoadAssetAtPath<Sprite>(assetPath);
            }

            so.Sprites = textureArray;
            so.TimeMultiplier = new float[textureArray.Length];
            for (i = 0; i < textureArray.Length; i++)
                so.TimeMultiplier[i] = 1;


            EditorUtility.SetDirty(so);
            AssetDatabase.CreateAsset(so, path + ".asset");

        }
    #endif

        private static IComparer _sortFilesByName()
        {      
           return (IComparer) new FileNameComparer();
        }
}