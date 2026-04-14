using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private Transform contentRoot;
    [SerializeField] private InventoryRowUI rowPrefab;

    void Start()
    {
        Refresh();
    }

    public void Refresh()
    {
        foreach (Transform child in contentRoot)
            Destroy(child.gameObject);

        List<CatchInventoryEntry> entries = CatchInventoryManager.GetAllEntries();

        foreach (var entry in entries)
        {
            if (entry.count <= 0)
                continue;

            InventoryRowUI row = Instantiate(rowPrefab, contentRoot);
            row.Setup(entry);
        }
    }
}