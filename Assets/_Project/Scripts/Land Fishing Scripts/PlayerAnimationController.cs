using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{

    Animator anim;
    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    //Starts the next step in the fishing process right after the animation is finished playing
    void Reeling()
    {
        Land_Fishing_Game_Manager.instance.Casting();
    }

}
