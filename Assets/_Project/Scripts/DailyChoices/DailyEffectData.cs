using UnityEngine;

[CreateAssetMenu(fileName = "DailyEffect", menuName = "Daily Effects/Daily Effect")]
public class DailyEffectData : ScriptableObject
{
    public string effectId;
    public string effectName;

    [TextArea]
    public string description;

    public Sprite icon;

    public DailyEffectType effectType = DailyEffectType.None;
    public bool isNegative = false;

    public int intValue = 0;
    public float floatValue = 0f;
}