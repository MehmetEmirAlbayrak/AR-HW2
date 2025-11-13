using System.Collections.Generic;
using UnityEngine;

public static class Registration
{
    public struct Rigid
    {
        public Matrix4x4 R;
        public Vector3 T;
        public float scale; // rigid için 1
    }

    // Üç noktadan yerel eksen üret
    static void FrameFromTriangle(Vector3 a, Vector3 b, Vector3 c, out Matrix4x4 F, out Vector3 centroid)
    {
        centroid = (a + b + c) / 3f;
        Vector3 v1 = (b - a).normalized;
        Vector3 tmp = (c - a);
        Vector3 v2 = (tmp - Vector3.Dot(tmp, v1) * v1).normalized;
        Vector3 v3 = Vector3.Cross(v1, v2).normalized;
        // Rotasyon matrisi sütunlarý eksenler olsun
        F = Matrix4x4.identity;
        F.SetColumn(0, new Vector4(v1.x, v1.y, v1.z, 0));
        F.SetColumn(1, new Vector4(v2.x, v2.y, v2.z, 0));
        F.SetColumn(2, new Vector4(v3.x, v3.y, v3.z, 0));
        F.SetColumn(3, new Vector4(0, 0, 0, 1));
    }

    // Üç P noktasý ve üç Q noktasý eþleþtirildi varsayýmýyla rigid dönüþüm
    public static Rigid EstimateRigidFromTriangles(Vector3 p1, Vector3 p2, Vector3 p3,
                                                  Vector3 q1, Vector3 q2, Vector3 q3)
    {
        FrameFromTriangle(p1, p2, p3, out var FP, out var cP);
        FrameFromTriangle(q1, q2, q3, out var FQ, out var cQ);

        // R = FP * FQ^T
        var FQ_T = FQ.transpose;
        Matrix4x4 Rm = FP * FQ_T;

        // T = cP - R * cQ
        Vector3 RcQ = Rm.MultiplyVector(cQ);
        Vector3 T = cP - RcQ;

        return new Rigid
        {
            R = Rm,
            T = T,
            scale = 1f
        };
    }

    public static Vector3 ApplyRigid(Vector3 v, Rigid rt)
    {
        return rt.R.MultiplyVector(v) + rt.T;
    }

    public static List<Vector3> ApplyRigid(List<Vector3> pts, Rigid rt)
    {
        var outPts = new List<Vector3>(pts.Count);
        foreach (var p in pts) outPts.Add(ApplyRigid(p, rt));
        return outPts;
    }
}
