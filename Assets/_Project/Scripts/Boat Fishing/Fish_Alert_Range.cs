using UnityEngine;

public class Fish_Alert_Range : MonoBehaviour
{
    public float alertRange = 1.25f;
    public CircleCollider2D alertCollider;
    public Boat_Fish fish;

    void Awake()
    {
        fish = GetComponentInParent<Boat_Fish>();
        alertCollider = GetComponent<CircleCollider2D>(); 
        alertCollider.radius = alertRange; 
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hook"))
        {
            Debug.Log("Fish is alerted to the hook!");
            fish.setChasingHook(true);
            fish.setHookPosition(collision.transform.position);
        }
    }
    public void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hook"))
        {
            fish.setHookPosition(collision.transform.position);
        }
    }
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hook"))
        {
            Debug.Log("Fish is no longer alerted to the hook.");
            fish.setChasingHook(false);
        }
    }
}
