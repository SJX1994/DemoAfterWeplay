using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DNA_PlayerData
{
    public class DnaData
    {
        public static bool isCollecting = false;
        public static bool IsCollecting
        {
            get
            {
                return isCollecting;
            }
            set
            {
                if(isCollecting == value)return;
                isCollecting = value;
            }
        }
        public enum Type_Position
        {
            notReady,
            up,
            down
        }
        public enum DNA_Type
        {
            notReady,
            black,
            white,
            yellow
        }
    }
}
