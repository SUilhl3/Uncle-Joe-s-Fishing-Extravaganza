using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DailyEffectChoiceSave
{
    public List<string> offeredEffectIds = new List<string>();
}

public class DailyEffectManager : MonoBehaviour
{
    public static DailyEffectManager Instance { get; private set; }

    [SerializeField] private List<DailyEffectData> positiveEffects = new List<DailyEffectData>();
    [SerializeField] private List<DailyEffectData> negativeEffects = new List<DailyEffectData>();

    [SerializeField] private DailyEffectData activeEffect;

    private const string ActiveEffectKey = "ActiveDailyEffectId";

    private string OfferKey =>
        "DailyEffectOffers_" + SaveManager.GetCurrentGameDate().ToString("yyyyMMdd");

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        LoadActiveEffect();
    }

    public DailyEffectData GetActiveEffect()
    {
        return activeEffect;
    }

    public bool HasEffect(DailyEffectType type)
    {
        return activeEffect != null && activeEffect.effectType == type;
    }

    public int GetIntValue(DailyEffectType type)
    {
        if (!HasEffect(type)) return 0;
        return activeEffect.intValue;
    }

    public float GetFloatValue(DailyEffectType type)
    {
        if (!HasEffect(type)) return 0f;
        return activeEffect.floatValue;
    }

    public void ClearActiveEffect()
    {
        activeEffect = null;
        PlayerPrefs.DeleteKey(ActiveEffectKey);
        PlayerPrefs.Save();
    }

    public List<DailyEffectData> GetOrGenerateChoicesForToday()
    {
        List<DailyEffectData> pool = IsNegativeDay()
            ? new List<DailyEffectData>(negativeEffects)
            : new List<DailyEffectData>(positiveEffects);

        string savedJson = PlayerPrefs.GetString(OfferKey, "");
        if (!string.IsNullOrEmpty(savedJson))
        {
            DailyEffectChoiceSave save = JsonUtility.FromJson<DailyEffectChoiceSave>(savedJson);
            if (save != null && save.offeredEffectIds.Count > 0)
            {
                List<DailyEffectData> loaded = new List<DailyEffectData>();
                foreach (string id in save.offeredEffectIds)
                {
                    DailyEffectData effect = FindEffectById(id);
                    if (effect != null)
                        loaded.Add(effect);
                }

                if (loaded.Count > 0)
                    return loaded;
            }
        }

        List<DailyEffectData> choices = new List<DailyEffectData>();
        int choiceCount = Mathf.Min(3, pool.Count);

        for (int i = 0; i < choiceCount; i++)
        {
            int index = UnityEngine.Random.Range(0, pool.Count);
            choices.Add(pool[index]);
            pool.RemoveAt(index);
        }

        DailyEffectChoiceSave newSave = new DailyEffectChoiceSave();
        foreach (var choice in choices)
            newSave.offeredEffectIds.Add(choice.effectId);

        PlayerPrefs.SetString(OfferKey, JsonUtility.ToJson(newSave));
        PlayerPrefs.Save();

        return choices;
    }

    public void ChooseEffect(DailyEffectData chosen)
    {
        if (chosen == null) return;

        activeEffect = chosen;
        PlayerPrefs.SetString(ActiveEffectKey, chosen.effectId);
        PlayerPrefs.Save();
    }

    public bool IsNegativeDay()
    {
        int day = SaveManager.GetDayNumber();
        return day % 5 == 0;
    }

    private void LoadActiveEffect()
    {
        string id = PlayerPrefs.GetString(ActiveEffectKey, "");
        if (string.IsNullOrEmpty(id))
        {
            activeEffect = null;
            return;
        }

        activeEffect = FindEffectById(id);
    }

    private DailyEffectData FindEffectById(string id)
    {
        foreach (var effect in positiveEffects)
        {
            if (effect != null && effect.effectId == id)
                return effect;
        }

        foreach (var effect in negativeEffects)
        {
            if (effect != null && effect.effectId == id)
                return effect;
        }

        return null;
    }
}