using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OffScreenMatrixUpdate : MonoBehaviour
{
    public Transform CAVETrans;

    private Vector3 p_a, p_b, p_c;
    private Vector3 v_r, v_u, v_n;
    private Vector3 v_a, v_b, v_c;
    private float zNear, zFar, dis;
    private float l, r, b, t;
    private Camera cam;
    // Start is called before the first frame update
    void Start()
    {
        zNear = 0.1f;
        zFar = 100f;

        cam = GetComponent<Camera>();
        p_a = CAVETrans.TransformPoint(new Vector3(5f, 0f, 5f));
        p_b = CAVETrans.TransformPoint(new Vector3(-5f, 0f, 5f));
        p_c = CAVETrans.TransformPoint(new Vector3(5f, 0f, -5f));

        v_r = Vector3.Normalize(p_b - p_a);
        v_u = Vector3.Normalize(p_c - p_a);
        v_n = Vector3.Normalize(Vector3.Cross(v_r, v_u));
     }

    // Update is called once per frame
    void Update()
    {
        v_a = p_a - transform.position;
        v_b = p_b - transform.position;
        v_c = p_c - transform.position;

        dis = -Vector3.Dot(v_n, v_a);

        l = Vector3.Dot(v_r, v_a) * zNear / dis;
        r = Vector3.Dot(v_r, v_b) * zNear / dis;
        b = Vector3.Dot(v_u, v_a) * zNear / dis;
        t = Vector3.Dot(v_u, v_c) * zNear / dis;

        Matrix4x4 P = Matrix4x4.Frustum(l, r, b, t, zNear, zFar);
        Matrix4x4 M_T = Matrix4x4.identity;
        M_T.SetRow(0, new Vector4(-v_r.x, -v_r.y, v_r.z, 0.0f));
        M_T.SetRow(1, new Vector4(-v_u.x, -v_u.y, v_u.z, 0.0f));
        M_T.SetRow(2, new Vector4(-v_n.x, -v_n.y, v_n.z, 0.0f));
        //M_T.SetRow(3, new Vector4(0.0f, 0.0f, 0.0f, 1.0f));

        cam.projectionMatrix = P * M_T * Matrix4x4.Translate(-transform.position);
    }
}
