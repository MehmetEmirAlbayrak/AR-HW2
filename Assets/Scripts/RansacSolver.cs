using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RansacSolver : MonoBehaviour
{
    [Header("RANSAC")]
    public int iterations = 500;
    public float inlierThreshold = 0.05f; // nokta birimine göre ayarla
    public int seed = 42;

    [HideInInspector] public List<Vector3> P;
    [HideInInspector] public List<Vector3> Q;

    public Registration.Rigid BestModel { get; private set; }
    public int BestInliers { get; private set; }

    void Awake() { Random.InitState(seed); }

    public void SetData(List<Vector3> p, List<Vector3> q) { P = p; Q = q; }

    int CountInliers(Registration.Rigid model)
    {
        int cnt = 0;
        foreach (var q in Q)
        {
            var qT = Registration.ApplyRigid(q, model);
            // naive en yakýn P
            float best = float.MaxValue;
            foreach (var p in P)
            {
                float d = (p - qT).sqrMagnitude;
                if (d < best) best = d;
            }
            if (Mathf.Sqrt(best) <= inlierThreshold) cnt++;
        }
        return cnt;
    }

    public void RunRansac()
    {
        if (P == null || Q == null || P.Count < 3 || Q.Count < 3)
            throw new Exception("Not enough points.");

        BestInliers = -1;

        for (int it = 0; it < iterations; it++)
        {
            // Üç rastgele benzersiz indeks
            var (i1, i2, i3) = PickTriple(P.Count);
            var (j1, j2, j3) = PickTriple(Q.Count);

            var model = Registration.EstimateRigidFromTriangles(
                P[i1], P[i2], P[i3],
                Q[j1], Q[j2], Q[j3]
            );

            int inl = CountInliers(model);
            if (inl > BestInliers)
            {
                BestInliers = inl;
                BestModel = model;
            }
        }
    }

    (int, int, int) PickTriple(int n)
    {
        int a = Random.Range(0, n);
        int b; do { b = Random.Range(0, n); } while (b == a);
        int c; do { c = Random.Range(0, n); } while (c == a || c == b);
        return (a, b, c);
    }
}
