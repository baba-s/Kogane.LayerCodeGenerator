using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;

namespace Kogane.Internal
{
    internal sealed class LayerCodeGeneratorSettingProvider : SettingsProvider
    {
        public const string PATH = "Kogane/Layer Code Generator";

        private Editor m_editor;

        private LayerCodeGeneratorSettingProvider
        (
            string              path,
            SettingsScope       scopes,
            IEnumerable<string> keywords = null
        ) : base( path, scopes, keywords )
        {
        }

        public override void OnActivate( string searchContext, VisualElement rootElement )
        {
            var instance = LayerCodeGeneratorSetting.instance;

            instance.hideFlags = HideFlags.HideAndDontSave & ~HideFlags.NotEditable;

            Editor.CreateCachedEditor( instance, null, ref m_editor );
        }

        public override void OnGUI( string searchContext )
        {
            using var changeCheckScope = new EditorGUI.ChangeCheckScope();

            if ( GUILayout.Button( "Generate" ) )
            {
                LayerCodeGenerator.Generate();
            }

            EditorGUILayout.Space();

            if ( GUILayout.Button( "Reset to Default" ) )
            {
                Undo.RecordObject( LayerCodeGeneratorSetting.instance, "Reset to Default" );
                LayerCodeGeneratorSetting.instance.ResetToDefault();
            }

            m_editor.OnInspectorGUI();

            if ( !changeCheckScope.changed ) return;

            LayerCodeGeneratorSetting.instance.Save();
        }

        [SettingsProvider]
        private static SettingsProvider CreateSettingProvider()
        {
            return new LayerCodeGeneratorSettingProvider
            (
                path: PATH,
                scopes: SettingsScope.Project
            );
        }
    }
}