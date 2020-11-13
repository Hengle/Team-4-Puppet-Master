using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR

using UnityEditor;

namespace SNAPSPRODUCTION
{

    public class LightmappedLODAssigner
    {

        [MenuItem("Tools/Snaps/Apply LightmappedLOD to all prefabs")]
        static void ApplyLightmappedLODsToAllPrefab()
        {

            int totalAssigned = 0;

            ArrayList assignedPrefabs = new ArrayList();

            assignedPrefabs.Clear();


            string[] guids = AssetDatabase.FindAssets("t:Prefab", new string[] { "Assets" });

            foreach (string guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);

                GameObject go = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);

                if (go != null)
                {

                    //Debug.Log("GameObject : " + go.name);
                    //Debug.Log("GameObject PATH : " + assetPath);

                    LODGroup[] lodGroups = go.GetComponentsInChildren<LODGroup>();

                    foreach (LODGroup lodGroup in lodGroups)
                    {
                        if (lodGroup.lodCount != 0 && lodGroup.lodCount > 1)
                        {
                            for (int i = 1; i < lodGroup.lodCount; i++)
                            {
                                if (lodGroup.GetLODs()[i].renderers.Length != 0)
                                {
                                    GameObject targetGo = lodGroup.GetLODs()[i].renderers[0].gameObject;

                                    if (targetGo != null)
                                    {
                                        LightmappedLOD lMappedLods = targetGo.GetComponent<LightmappedLOD>();

                                        if (lMappedLods == null)
                                        {
                                            targetGo.AddComponent<LightmappedLOD>();
                                            assignedPrefabs.Add(assetPath + " : " + targetGo.name);
                                            totalAssigned++;
                                            AssetDatabase.SaveAssets();
                                        }
                                    }

                                }

                            }
                        }
                    }

                }

            }

            // Display result

            EditorWindow sceneView = EditorWindow.GetWindow<SceneView>();

            if (sceneView != null)
            {
                sceneView.ShowNotification(new GUIContent(string.Format("Assigned prefabs / Total prefabs : {0} / {1}", totalAssigned, guids.Length) ));
            }


            Debug.Log("### LightmappedLOD assigned status ###");

            foreach (string assignedPrefab in assignedPrefabs)
            {
                Debug.Log(assignedPrefab);
            }

            Debug.Log(string.Empty);
        }

    }
}
#endif