 using UnityEngine;
 using UnityEditor;
 
 [CustomEditor(typeof(ImageAnimator))]
 public class MyScriptEditor : Editor
 {
     ImageAnimator mTarget;
     
     private void CallbackFunction()
     {
        if (mTarget != null && mTarget.Playing)
        {
            // Debug.Log("Updating");
            EditorUtility.SetDirty(target);
        }  
     }
     public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        
        if(GUILayout.Button("Run Animation"))
        {
            mTarget = target as ImageAnimator;
            EditorApplication.update += CallbackFunction;
            mTarget.StartAnimation();
        }
    }
     void OnEnable()
     {
         EditorApplication.update += CallbackFunction;
     }
     void OnDisable()
     {
         EditorApplication.update -= CallbackFunction;
         if (mTarget != null)  mTarget.StopAnimation();
     }
 }