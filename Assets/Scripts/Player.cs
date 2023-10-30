using Mirror;
using UnityEngine;

public class Player : NetworkBehaviour
{
    public float speed = 30;
    public Rigidbody rigid;

    // need to use FixedUpdate for rigidbody
    void FixedUpdate()
    {
        //
        // only let the local player control the racket.
        // don't control other player's rackets
        if (isLocalPlayer)
            rigid.velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")) * speed * Time.fixedDeltaTime;
    }
}
