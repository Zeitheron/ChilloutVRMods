using UnityEngine;

namespace OscCore
{
    [AddComponentMenu("OSC/Input/String Input")]
    public class OscStringMessageHandler : OscMessageHandler<string, StringUnityEvent>
    {
        protected override void ValueRead(OscMessageValues values, string addr)
        {
            m_Value = values.ReadStringElement(0);
        }
    }
}
