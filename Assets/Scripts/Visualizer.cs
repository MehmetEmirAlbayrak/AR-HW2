using System.Collections.Generic;
using UnityEngine;

public class Visualizer : MonoBehaviour
{
    [Header("Point Prefab")]
    public GameObject pointPrefab; // küçük sphere (öneri: 0.02 scale)
    public Transform parentP;
    public Transform parentQ;
    public Transform parentQAligned;

    [Header("Line Material")]
    public Material lineMat;

    [HideInInspector] public List<Vector3> P, Q, QAligned;
    List<GameObject> spawned = new();

    public enum ViewMode { PointsOnly, MotionLines }
    public ViewMode viewMode = ViewMode.PointsOnly;

    public void ClearAll()
    {
        foreach (var go in spawned) Destroy(go);
        spawned.Clear();
        P = Q = QAligned = null;
    }

    public void ShowPoints(List<Vector3> p, List<Vector3> q, List<Vector3> qAligned)
    {
        P = p; Q = q; QAligned = qAligned;

        // Renkler: P-kýrmýzý, Q-mavi, Q' aligned-yeþil
        SpawnSet(P, parentP, Color.red);
        SpawnSet(Q, parentQ, Color.blue);
        SpawnSet(QAligned, parentQAligned, Color.green);

        if (viewMode == ViewMode.MotionLines)
            DrawMotionLines(Q, QAligned);
    }

    void SpawnSet(List<Vector3> pts, Transform parent, Color color)
    {
        foreach (var v in pts)
        {
            var go = Instantiate(pointPrefab, v, Quaternion.identity, parent);
            var mr = go.GetComponent<Renderer>();
            if (mr) { mr.material = new Material(mr.material); mr.material.color = color; }
            spawned.Add(go);
        }
    }

    void DrawMotionLines(List<Vector3> q0, List<Vector3> q1)
    {
        int n = Mathf.Min(q0.Count, q1.Count);
        for (int i = 0; i < n; i++)
        {
            var go = new GameObject("Line_" + i);
            go.transform.SetParent(transform);
            var lr = go.AddComponent<LineRenderer>();
            lr.material = lineMat;
            lr.positionCount = 2;
            lr.startWidth = lr.endWidth = 0.01f;
            lr.useWorldSpace = true;
            lr.SetPosition(0, q0[i]);
            lr.SetPosition(1, q1[i]);
            spawned.Add(go);
        }
    }

    public void ToggleView()
    {
        viewMode = (viewMode == ViewMode.PointsOnly) ? ViewMode.MotionLines : ViewMode.PointsOnly;
        // yeniden çiz
        ClearAll();
    }
}
