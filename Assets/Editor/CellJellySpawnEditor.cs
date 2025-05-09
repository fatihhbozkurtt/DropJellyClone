using Controllers;
using EssentialManagers.Packages.GridManager.Scripts;
using UnityEditor;
using UnityEngine;

namespace Editor
{
    [CustomEditor(typeof(CellController))]
    [CanEditMultipleObjects]
    public class CellJellySpawnEditor : UnityEditor.Editor
    {
        private SerializedProperty spawnablePieceDataListProp;

        private void OnEnable()
        {
            spawnablePieceDataListProp = serializedObject.FindProperty("spawnablePieceDataList");
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            base.OnInspectorGUI();

            GUILayout.Space(10);

            EditorGUILayout.LabelField("🧪 Jelly Spawn Tools", EditorStyles.boldLabel);
            EditorGUILayout.PropertyField(spawnablePieceDataListProp, true);

            GUILayout.Space(5);
            GUI.backgroundColor = Color.green;
            if (GUILayout.Button("Spawn Jelly On Selected Cells"))
            {
                foreach (Object t in targets)
                {
                    CellController cell = t as CellController;
                    if (cell != null)
                        SpawnJellyAtCell(cell);
                }
            }

            GUILayout.Space(5);
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Destroy Jelly On Selected Cells"))
            {
                foreach (Object t in targets)
                {
                    CellController cell = t as CellController;
                    if (cell != null)
                        DestroyJellyAtCell(cell);
                }
            }

            GUI.backgroundColor = Color.white;
            serializedObject.ApplyModifiedProperties();
        }

        private void SpawnJellyAtCell(CellController cell)
        {
            if (cell == null || cell.GetOccupantJB() != null) return;

            GameObject prefab = GetOccupierPrefab(cell);
            if (prefab == null)
            {
                Debug.LogError($"[{cell.name}] Occupier prefab is not assigned.");
                return;
            }

            GameObject jellyObj = (GameObject)PrefabUtility.InstantiatePrefab(prefab, cell.transform);
            jellyObj.transform.localPosition = Vector3.zero;

            JellyBlock jellyBlock = jellyObj.GetComponent<JellyBlock>();
            if (jellyBlock != null)
            {
                jellyBlock.SetCell(cell); // Hücre bağlantısını yap
                jellyBlock.SetInnerPieces(cell.GetSpawnablePieceDataList()); // InnerPieceData listesi aktar
                cell.SetOccupied(jellyBlock);
            }
            else
            {
                Debug.LogError("Spawned object is missing JellyBlock component.");
            }

            Undo.RegisterCreatedObjectUndo(jellyObj, "Spawn JellyBlock");
            EditorUtility.SetDirty(cell);
        }

        private void DestroyJellyAtCell(CellController cell)
        {
            JellyBlock jb = cell.GetOccupantJB();
            if (jb == null) return;

            Undo.DestroyObjectImmediate(jb.gameObject);
            cell.SetFree();
            EditorUtility.SetDirty(cell);
        }

        private GameObject GetOccupierPrefab(CellController cell)
        {
            var field = typeof(CellController).GetField("occupierObjectPrefab",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            return field?.GetValue(cell) as GameObject;
        }
    }
}
