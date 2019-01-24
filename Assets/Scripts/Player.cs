using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Racer
{
    public Camera cam;
    public override void Move()
    {
        base.Move();
        //speed = Input.GetAxis("Vertical");
        right_offset = Mathf.Clamp(right_offset + Input.GetAxis("Horizontal") * Time.deltaTime * 10, -1, 1);
        cam.transform.position = Vector3.Lerp(cam.transform.position, transform.position - transform.forward * 2 + transform.up, Time.deltaTime*8);
        cam.transform.rotation = Quaternion.Lerp(cam.transform.rotation, Quaternion.LookRotation(transform.position + Vector3.up - cam.transform.position), Time.deltaTime * 8);
    }
    public override void UpdateProgress()
    {
        progress += Input.GetAxis("Vertical") * Time.deltaTime * speed / track.path.nodes[segment].get_dist_to_next(right_offset);
    }
}
