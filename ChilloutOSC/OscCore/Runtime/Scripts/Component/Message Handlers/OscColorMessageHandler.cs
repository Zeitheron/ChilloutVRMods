using UnityEngine;

namespace OscCore
{
    [AddComponentMenu("OSC/Input/Color Input")]
    public class OscColorMessageHandler : OscMessageHandler<Color, ColorUnityEvent>
    {
        protected override void ValueRead(OscMessageValues values, string addr)
        {
            m_Value = values.ReadColor32Element(0);
        }
    }
}
