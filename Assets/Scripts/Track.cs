using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class Track : MonoBehaviour
{

    public Path path;
    public List<Transform> nodes = new List<Transform>();
    public List<Racer> racers = new List<Racer>();

    private void Start()
    {
        InitPath();
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if (path == null)
        {
            return;
        }
        Color c = Gizmos.color;
        for (int i = 0; i < path.nodes.Count; i++)
        {
            Gizmos.color = Color.red;
            for (int j = 0; j < path.nodes[i].dist_to_next_left; j++)
            {
                Gizmos.DrawLine(path.GetPosPath(i, j / path.nodes[i].dist_to_next_left, -1), path.GetPosPath(i, (j + 1) / path.nodes[i].dist_to_next_left, -1));
            }
            Gizmos.color = Color.green;
            for (int j = 0; j < path.nodes[i].dist_to_next_right; j++)
            {
                Gizmos.DrawLine(path.GetPosPath(i, j / path.nodes[i].dist_to_next_right, 1), path.GetPosPath(i, (j + 1) / path.nodes[i].dist_to_next_right, 1));
            }
        }
        Gizmos.color = c;
    }
#endif

    public void InitPath()
    {
        path = new Path();
        List<Vector3> forwards = new List<Vector3>();
        List<Vector3> rights = new List<Vector3>();
        for (int i = 0; i < nodes.Count; i++)
        {
            Transform cur = nodes[i], next = nodes[(i + 1) % nodes.Count];
            Transform pos_cur = cur.Find("Point");
            Transform pos_next = next.Find("Point");
            Vector3 forward = (pos_next.position - pos_cur.position).normalized;
            Vector3 right = Vector3.Cross(forward, -pos_cur.up).normalized * 3;
            forwards.Add(forward);
            rights.Add(right*(i%2==0?1:2));
        }

        for (int i = 0; i < nodes.Count; i++)
        {
            int next_index = (i + 1) % nodes.Count;
            Transform cur = nodes[i], next = nodes[next_index];
            Transform pos_cur = cur.Find("Point");
            Transform anchor_cur_A = cur.Find("Anchor 1");
            Transform anchor_cur_B = cur.Find("Anchor 2");
            Transform pos_next = next.Find("Point");
            Transform anchor_next_A = next.Find("Anchor 1");
            Transform anchor_next_B = next.Find("Anchor 2");
            Vector3 forward = forwards[i];
            Vector3 right = rights[i];
            Vector3 forward_next = -forwards[next_index];
            Vector3 right_next = rights[next_index];
            Vector3 pos_left = pos_cur.position;
            Vector3 pos_right = pos_cur.position + right + forward * 3;
            Vector3 pos_next_left = pos_next.position;
            Vector3 pos_next_right = pos_next.position + right_next - forward_next * 3;
            Vector3 anchor_a_left = anchor_cur_A.position;
            Vector3 anchor_a_right = anchor_cur_A.position + right + forward * 3;
            Vector3 anchor_b_left = anchor_cur_B.position;
            Vector3 anchor_b_right = anchor_cur_B.position + right + forward * 3;
            Vector3 anchor_a_left_next = anchor_next_A.position;
            Vector3 anchor_a_right_next = anchor_next_A.position + right_next - forward_next * 3;
            Vector3 anchor_b_left_next = anchor_next_B.position;
            Vector3 anchor_b_right_next = anchor_next_B.position + right_next - forward_next * 3;
            float dist_to_next_left = Vector3.Distance(pos_left, pos_next_left);
            float dist_to_next_right = Vector3.Distance(pos_right, pos_next_right);
            path.nodes.Add(new Path.Node()
            {
                left = pos_left,
                right = pos_right,
                next_left = pos_next_left,
                next_right = pos_next_right,
                anchorALeft = anchor_a_left,
                anchorARight = anchor_a_right,
                anchorBLeft = anchor_b_left,
                anchorBRight = anchor_b_right,
                anchorALeftNext = anchor_a_left_next,
                anchorARightNext = anchor_a_right_next,
                anchorBLeftNext = anchor_b_left_next,
                anchorBRightNext = anchor_b_right_next,
                dist_to_next_left = dist_to_next_left,
                dist_to_next_right = dist_to_next_right
            });
        }
    }

    private void LateUpdate()
    {
        for (int i = 0; i < racers.Count; i++)
        {
            racers[i].Rotate();
            racers[i].Move();
        }
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(Track))]
public class TrackEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Track track = target as Track;
        if (GUILayout.Button("Init path"))
        {
            track.InitPath();
        }
        if(track==null)
        {
            return;
        }
        if (GUILayout.Button("Spawn spheres"))
        {
            CreateSpheres();
        }
    }

    public void CreateSpheres()
    {
        GameObject sphere_track = new GameObject();
        sphere_track.name = "Sphere track";
        Track track = target as Track;
        Path path = track.path;
        for (int i = 0; i < path.nodes.Count; i++)
        {
            for (int j = 0; j < path.nodes[i].dist_to_next_left; j++)
            {
                Transform sphere_left = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                sphere_left.position = path.GetPosPath(i, j / path.nodes[i].dist_to_next_left, -1);
                sphere_left.SetParent(sphere_track.transform);
                sphere_left.name = i + "_" + j;
                Transform sphere_right = GameObject.CreatePrimitive(PrimitiveType.Sphere).transform;
                sphere_right.position = path.GetPosPath(i, j / path.nodes[i].dist_to_next_left, 1);
                sphere_right.SetParent(sphere_track.transform);
                sphere_right.name = i + "_" + j;
            }
        }
    }
}
#endif