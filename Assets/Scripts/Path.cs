using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path {
    
    public struct Node
    {
        public Vector3 left, next_left, right, next_right;
        public Vector3 anchorALeft, anchorARight, anchorBLeft, anchorBRight;
        public Vector3 anchorALeftNext, anchorARightNext, anchorBLeftNext, anchorBRightNext;
        public float dist_to_next_left;
        public float dist_to_next_right;

        public Vector3 get_pos(float right_offset)
        {
            return Vector3.Lerp(left, right, (right_offset + 1) / 2);
        }

        public Vector3 get_next_pos(float right_offset)
        {
            return Vector3.Lerp(next_left, next_right, (right_offset + 1) / 2);
        }

        public Vector3 get_AnchorA(float right_offset)
        {
            return Vector3.Lerp(anchorALeft, anchorARight, (right_offset + 1) / 2);
        }

        public Vector3 get_AnchorB(float right_offset)
        {
            return Vector3.Lerp(anchorBLeft, anchorBRight, (right_offset + 1) / 2);
        }

        public Vector3 get_AnchorANext(float right_offset)
        {
            return Vector3.Lerp(anchorALeftNext, anchorARightNext, (right_offset + 1) / 2);
        }

        public Vector3 get_AnchorBNext(float right_offset)
        {
            return Vector3.Lerp(anchorBLeftNext, anchorBRightNext, (right_offset + 1) / 2);
        }

        public float get_dist_to_next(float right_offset)
        {
            return Mathf.Lerp(dist_to_next_left, dist_to_next_right, (right_offset + 1) / 2);
        }
    }

    public List<Node> nodes = new List<Node>();

    public Vector3 GetPosPath(int index, float progress, float right_offset)
    {
        Node cur = nodes[index];
        return Bezier.EvaluateCubic(cur.get_pos(right_offset), cur.get_AnchorA(right_offset) , cur.get_AnchorBNext(right_offset), cur.get_next_pos(right_offset), progress);
    }
}
