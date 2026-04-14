using System;
using UnityEngine;

[Serializable]
public class TreasureBuyerCatalogEntry
{
    public string id;
    public string displayName;
    public int baseValue;
    public string unlockKey; // leave blank for always-available items
}