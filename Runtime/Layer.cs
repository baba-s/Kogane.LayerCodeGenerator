using System;
using UnityEngine;

namespace Kogane
{
    [Serializable]
    public struct Layer
    {
        [SerializeField] private string m_name;
        [SerializeField] private int    m_index;
        [SerializeField] private int    m_mask;

        public string Name  => m_name;
        public int    Index => m_index;
        public int    Mask  => m_mask;

        public Layer( string name )
        {
            m_name  = name;
            m_index = LayerMask.NameToLayer( name );
            m_mask  = LayerMask.GetMask( name );
        }

        public override string ToString()
        {
            return JsonUtility.ToJson( this, true );
        }
    }
}