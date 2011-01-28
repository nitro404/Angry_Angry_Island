using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Firecracker_Engine
{
    public delegate void UICallBack (string ID);

    public static class IDRegistrar
    {
        private static Dictionary<string, UICallBack> m_registeredIDs = new Dictionary<string, UICallBack>();

        public static void RegisterID(string ID, UICallBack callBack)
        {
            m_registeredIDs.Add(ID, callBack);
        }

        public static void ExecuteActionCallBack(string ID)
        {
            if (m_registeredIDs.ContainsKey(ID))
            {
                m_registeredIDs[ID](ID);
            }
        }


    }
}
