using System;
using System.Collections.Generic;

[Serializable] public class SaveData
{
    public string playerName;
    public int money;
    public int day;
    public CatchInventorySaveData inventory;
    public int fishCaughtToday;
    public bool hasChest;
    public bool hasBait;
    public bool hasBait2;
    public int slotIndex;
}