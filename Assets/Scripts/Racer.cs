using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Racer : MonoBehaviour
{

    public Track track;
    public float progress;
    public int segment = 0;
    public float right_offset;
    protected float speed = 50;
    Vector3 lastPos;
    public Vector3 velocity
    {
        get { return track.path.GetPosPath(segment, progress + speed/10, right_offset)-track.path.GetPosPath(segment, progress, right_offset); }
    }

    private void Start()
    {
        lastPos = transform.position;
        right_offset = Random.Range(-1f, 1f);
        progress = Random.Range(0f, 0.1f);
    }

    public void Rotate()
    {
        Vector3 vel = velocity;
        if (vel != Vector3.zero)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel), Time.deltaTime*8);
        }
    }

    public virtual void UpdateProgress()
    {
        progress += Time.deltaTime * speed / track.path.nodes[segment].get_dist_to_next(right_offset);
    }

    public virtual void Move()
    {
        lastPos = transform.position;
        transform.position = track.path.GetPosPath(segment, progress, right_offset) + Vector3.up * Mathf.Sin(Time.time)*0.25f;
        UpdateProgress();
        if (progress > 1)
        {
            progress = 0;
            segment = (segment + 1) % track.path.nodes.Count;
        }
        if (progress < 0)
        {
            progress = 1;
            segment = (segment - 1);
            if (segment < 0)
            {
                segment += track.path.nodes.Count;
            }
        }
    }
}
