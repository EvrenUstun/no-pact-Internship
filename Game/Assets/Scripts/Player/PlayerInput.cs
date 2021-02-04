using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    Player player;

    // Start is called before the first frame update
    void Start()
    {
        player = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        
        //Eger z�plama tusuna basarsak z�plama fonksiyonunu cag�r.
        if (Input.GetButtonDown("Jump"))
        {
            player.Jump();
        }
        else if (Input.GetButtonDown("Jump") && !player.isGrounded && player.canDash)
        {
            player.ForwardDash();
        }

       /* if (Input.GetButtonDown("Fire1"))
        {
            player.ShootProjectile();
        }*/
    }
}

