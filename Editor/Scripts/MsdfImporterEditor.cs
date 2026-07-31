using UnityEditor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace Nessie.MSDF
{
    [CustomEditor(typeof(MsdfImporter))]
    [CanEditMultipleObjects]
    public class MsdfImporterEditor : ScriptedImporterEditor
    {
        private SerializedProperty _propGeneratorMode;
        private SerializedProperty _propHeight;
        private SerializedProperty _propTextureType;

        private SerializedProperty _propWidth;
        private SerializedProperty _propWrapMode;

        public override void OnEnable()
        {
            base.OnEnable();

            InitializeProperties();
        }

        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            using (new EditorGUILayout.HorizontalScope())
            {
                EditorGUILayout.LabelField("Size");
                EditorGUILayout.PropertyField(_propWidth, GUIContent.none, GUILayout.MinWidth(40));
                EditorGUILayout.PropertyField(_propHeight, GUIContent.none, GUILayout.MinWidth(40));
            }

            EditorGUILayout.IntPopup(_propTextureType, Styles.TypeOptions, Styles.TypeValues);
            EditorGUILayout.IntPopup(_propGeneratorMode, Styles.ModeOptions, Styles.ModeValues);
            EditorGUILayout.PropertyField(_propWrapMode);

            serializedObject.ApplyModifiedProperties();
            ApplyRevertGUI();
            //base.OnInspectorGUI();
        }

        private void InitializeProperties()
        {
            _propWidth = serializedObject.FindProperty("width");
            _propHeight = serializedObject.FindProperty("height");
            _propTextureType = serializedObject.FindProperty("textureType");
            _propGeneratorMode = serializedObject.FindProperty("generatorMode");
            _propWrapMode = serializedObject.FindProperty("wrapMode");
        }

        private static class Styles
        {
            public static readonly GUIContent[] TypeOptions =
            {
                new("Default"),
                new("Sprite (2D and UI)"),
            };

            public static readonly int[] TypeValues =
            {
                (int)TextureType.Texture2D,
                (int)TextureType.Sprite,
            };

            public static readonly GUIContent[] ModeOptions =
            {
                new("SDF"),
                new("PSDF"),
                new("MSDF"),
                new("MTSDF"),
            };

            public static readonly int[] ModeValues =
            {
                (int)GeneratorMode.SDF,
                (int)GeneratorMode.PSDF,
                (int)GeneratorMode.MSDF,
                (int)GeneratorMode.MTSDF,
            };
        }
    }
}