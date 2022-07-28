using UnityEngine;

namespace OscCore
{
    [AddComponentMenu("OSC/Input/Integer Input")]
    public class OscIntMessageHandler : OscMessageHandler<int, IntUnityEvent>
    {
        protected override void ValueRead(OscMessageValues values, string addr)
        {
            m_Value = values.ReadIntElement(0);
        }
    }
}
