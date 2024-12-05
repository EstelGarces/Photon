using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

public class MovimientoScript : MonoBehaviourPunCallbacks
{
    public float velocidad = 5;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector3 movement = new Vector3(horizontal, 0, vertical);
            transform.Translate(movement * velocidad * Time.deltaTime, Space.World);

        }
    }
}
