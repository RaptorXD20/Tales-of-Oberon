using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//classe base per ogni arma e oggetto passivo, e sta alla base di WeaponData e PassiveItemData

public class ItemData : ScriptableObject
{
    public Sprite icon;
    public int maxLevel;

    [System.Serializable]
    public struct Evolution{
        public string name;

        public enum Condition{ auto, tresureChest}
        public Condition condition;

        [System.Flags] public enum Consumption{passives = 1, weapons = 2}
        public Consumption consumes;

        public int evolutionLevel;
        public Config[] catalyst;
        public Config outcome;

        [System.Serializable]
        public struct Config{
            public ItemData itemType;
            public int level;
        }
    }

    public Evolution[] evolutionData;
}
