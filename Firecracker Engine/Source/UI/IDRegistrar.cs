using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    public delegate void UICallBack (UICallBackInfo callbackInfo);

    public struct UICallBackInfo
    {
        public string ID;
        public UIEventType eventType;
        public int switcherIndex;
        public float sliderValue;
    }

    public enum UIEventType
    {
        ButtonAction,
        SwitcherInit,
        SwitcherChangeValue,
        SliderInit,
        SliderChangeValue
    }

    public static class IDRegistrar
    {
        private static Dictionary<string, UICallBack> m_registeredIDs = new Dictionary<string, UICallBack>();

        public static void RegisterID(string ID, UICallBack callBack)
        {
            m_registeredIDs.Add(ID, callBack);
        }

        public static void ExecuteCallBack(UICallBackInfo info)
        {
            if (m_registeredIDs.ContainsKey(info.ID))
            {
                m_registeredIDs[info.ID](info);
            }
        }


    }
}
