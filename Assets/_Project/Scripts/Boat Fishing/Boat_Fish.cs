using UnityEngine;


public class Boat_Fish : MonoBehaviour
{
    //temporary starting script, going to be overhauled later on
    //this is just going to do the basic fish movement and behavior for now

    public float swimSpeed = 2f;
    public Vector2 moveDirection;
    public GameObject fish;

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Hook"))
        {
            transform.SetParent(collision.transform); //make the fish a child of the hook so it moves with it
            transform.localPosition = new Vector3(-3f, 0, 0f);
        }
    }
}
