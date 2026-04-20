using UnityEngine;

public class LoadMenuManager : MonoBehaviour 
{ 
    public GameObject slotPrefab;
    public Transform contentParent;
    public int numberOfSlots = 3;
    
    void Start() 
    {
        PopulateSlots();
    }

    public void PopulateSlots()
    {
        foreach (Transform child in contentParent)
            Destroy(child.gameObject);

        Debug.Log("numberOfSlots = " + numberOfSlots);

        for (int i = 0; i < numberOfSlots; i++)
        {
            Debug.Log("Creating slot " + i);
            GameObject slotObj = Instantiate(slotPrefab, contentParent);
            slotObj.GetComponent<SaveSlotUI>().Setup(i);
        }
    }

    void OnEnable()
    {
        PopulateSlots();
    }
}