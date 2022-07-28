using UnityEngine;

namespace OscCore
{
    [AddComponentMenu("OSC/Input/Double Input")]
    public class OscFloat64MessageHandler : OscMessageHandler<double, DoubleUnityEvent>
    {
        protected override void ValueRead(OscMessageValues values, string addr)
        {
            m_Value = values.ReadFloat64Element(0);
        }
    }
}
