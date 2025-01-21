using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera_follow : MonoBehaviour
{   
    // script that follow the player wherever it moves
    public Vector3 offset = new Vector3(0, 0, -20);
    public Transform player;
    public float smoothSpeed = 0.5f;

    void FixedUpdate()
    {
        Vector3 desiredPosition = player.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, -10);

        transform.LookAt(player);
    }
}
