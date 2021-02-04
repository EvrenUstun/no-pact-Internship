using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBobInput : MonoBehaviour
{

    PlayerBob player;
    
    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<PlayerBob>();
    }

    // Update is called once per frame
    void Update()
    {
        //Eger zýplama tusuna basarsak zýplama fonksiyonunu cagýr.
        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }
        else if (Input.GetButtonDown("Jump") && !player.isGrounded && player.canGroundBreaker)
        {
            player.GroundBreaker();
        }
    }
}
