using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace GhostBox_PlayerData
{
    public class HodingObjectData
    {
        public enum HodingObjectType
        {
            box,
            timer,
            bomb,
        }
    }
    public class NPC_Data
    {
        public enum NPC_Type
        {
            notReady,
            good,
            bad,
            
        }
    }
    public class Referee
    {
        private static int round = 0;
        public static int Round
        {
            get
            {
                return round;
            }
            set
            {
                round = value;
                OnRoundChange?.Invoke(round);
            }
        }
        public static UnityAction<int> OnRoundChange;
    }
}