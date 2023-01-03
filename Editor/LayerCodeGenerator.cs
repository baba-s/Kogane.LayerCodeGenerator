using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace Kogane.Internal
{
    internal sealed class LayerCodeGenerator : AssetPostprocessor
    {
        private sealed class Data
        {
            private readonly string m_layerName;
            private readonly string m_memberName;

            public Data( string layerName )
            {
                m_layerName  = layerName;
                m_memberName = ToMemberName( layerName );
            }

            public string Replace( string template )
            {
                return template
                        .Replace( "%LAYER_NAME%", m_layerName )
                        .Replace( "%MEMBER_NAME%", m_memberName )
                    ;
            }
        }

        private static readonly Regex m_memberNameRefex = new( "[^0-9a-zA-Z_]+" );

        public static void Generate()
        {
            var setting = LayerCodeGeneratorSetting.instance;

            var dataArray = InternalEditorUtility.layers
                    .Select( x => new Data( x ) )
                    .ToArray()
                ;

            var fieldCode     = dataArray.Select( x => x.Replace( setting.FieldTemplate ) ).ToArray();
            var listValueCode = dataArray.Select( x => x.Replace( setting.ListValueTemplate ) ).ToArray();

            var sourceCode = setting.SourceTemplate
                    .Replace( "%FIELD_LIST%", string.Join( "\n", fieldCode ) )
                    .Replace( "%LIST_VALUE_LIST%", string.Join( "\n", listValueCode ) )
                ;

            var outputPath = setting.OutputPath;

            if ( !outputPath.StartsWith( "Assets/" ) )
            {
                Debug.LogWarning( "[Kogane.LayerCodeGenerator] Output Path は「Assets/」から始まる必要があります" );
                return;
            }

            var directoryName = Path.GetDirectoryName( outputPath );

            if ( !string.IsNullOrWhiteSpace( directoryName ) )
            {
                Directory.CreateDirectory( directoryName );
            }

            File.WriteAllText( outputPath, sourceCode, Encoding.UTF8 );
            AssetDatabase.Refresh();

            var textAsset = AssetDatabase.LoadAssetAtPath<TextAsset>( outputPath );
            EditorGUIUtility.PingObject( textAsset );
        }

        private static string ToMemberName( string layerName )
        {
            var memberName = m_memberNameRefex.Replace( layerName, "" );

            return char.IsNumber( memberName[ 0 ] )
                    ? $"_{memberName}"
                    : memberName
                ;
        }

        private static void OnPostprocessAllAssets
        (
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths
        )
        {
            if ( !LayerCodeGeneratorSetting.instance.IsAutoGenerate ) return;

            const string path = "ProjectSettings/TagManager.asset";

            var isChangedScenes = importedAssets.Contains( path );

            if ( !isChangedScenes ) return;

            Generate();
        }
    }
}