using UnityEditor;
using UnityEngine;

namespace Kogane.Internal
{
    [FilePath( "ProjectSettings/Kogane/LayerCodeGenerator.asset", FilePathAttribute.Location.ProjectFolder )]
    internal sealed class LayerCodeGeneratorSetting : ScriptableSingleton<LayerCodeGeneratorSetting>
    {
        private const bool   DEFAULT_IS_AUTO_GENERATE = true;
        private const string DEFAULT_OUTPUT_PATH      = "Assets/Layers.Generated.cs";

        private const string DEFAULT_SOURCE_TEMPLATE = @"using System.Collections.Generic;

namespace Kogane
{
    public static partial class Layers
    {
%FIELD_LIST%

        public static IReadOnlyList<Layer> All { get; } = new[]
        {
%LIST_VALUE_LIST%
        };
    }
}";

        private const string DEFAULT_FIELD_TEMPLATE      = @"        public static Layer %MEMBER_NAME% { get; } = new Layer( ""%LAYER_NAME%"" );";
        private const string DEFAULT_LIST_VALUE_TEMPLATE = @"            %MEMBER_NAME%,";

        [SerializeField]                    private bool   m_isAutoGenerate    = DEFAULT_IS_AUTO_GENERATE;
        [SerializeField]                    private string m_outputPath        = DEFAULT_OUTPUT_PATH;
        [SerializeField][TextArea( 1, 17 )] private string m_sourceTemplate    = DEFAULT_SOURCE_TEMPLATE;
        [SerializeField]                    private string m_fieldTemplate     = DEFAULT_FIELD_TEMPLATE;
        [SerializeField]                    private string m_listValueTemplate = DEFAULT_LIST_VALUE_TEMPLATE;

        public bool   IsAutoGenerate    => m_isAutoGenerate;
        public string SourceTemplate    => m_sourceTemplate;
        public string FieldTemplate     => m_fieldTemplate;
        public string ListValueTemplate => m_listValueTemplate;
        public string OutputPath        => m_outputPath;

        public void Save()
        {
            Save( true );
        }

        public void ResetToDefault()
        {
            m_isAutoGenerate    = DEFAULT_IS_AUTO_GENERATE;
            m_outputPath        = DEFAULT_OUTPUT_PATH;
            m_sourceTemplate    = DEFAULT_SOURCE_TEMPLATE;
            m_fieldTemplate     = DEFAULT_FIELD_TEMPLATE;
            m_listValueTemplate = DEFAULT_LIST_VALUE_TEMPLATE;
        }
    }
}