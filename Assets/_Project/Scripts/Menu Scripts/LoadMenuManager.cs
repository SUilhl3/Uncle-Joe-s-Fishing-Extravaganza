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
    
    void PopulateSlots()
    {
        for (int i = 0; i < numberOfSlots; i++) 
        {
            GameObject slotObj = Instantiate(slotPrefab, contentParent);
            SaveSlotUI slotUI = slotObj.GetComponent<SaveSlotUI>();
            slotUI.Setup(i);
        }
    }
}