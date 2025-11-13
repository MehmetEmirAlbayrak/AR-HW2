using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    public RansacSolver solver;
    public Visualizer vis;
    public TMP_Text resultText;

    // Dosya adlarý
    public string fileP = "points_P.txt";
    public string fileQ = "points_Q.txt";

    List<Vector3> P, Q, QAligned;

    public void Btn_LoadAndShow()
    {
        vis.ClearAll();

        P = PointLoader.LoadFromStreaming(fileP);
        Q = PointLoader.LoadFromStreaming(fileQ);

        solver.SetData(P, Q);

        // Ýlk durumda hizalanmamýþ QAligned = Q
        QAligned = new List<Vector3>(Q);
        vis.ShowPoints(P, Q, QAligned);

        resultText.text = $"Loaded P:{P.Count}, Q:{Q.Count}\nReady.";
    }

    public void Btn_RansacAlign()
    {
        solver.RunRansac();
        var M = solver.BestModel;

        QAligned = Registration.ApplyRigid(Q, M);
        vis.ShowPoints(P, Q, QAligned);

        var R = M.R;
        string rtxt =
            $"[{R[0, 0]:F3} {R[0, 1]:F3} {R[0, 2]:F3}]\n" +
            $"[{R[1, 0]:F3} {R[1, 1]:F3} {R[1, 2]:F3}]\n" +
            $"[{R[2, 0]:F3} {R[2, 1]:F3} {R[2, 2]:F3}]";

        resultText.text = $"Inliers: {solver.BestInliers}/{Q.Count}\n" +
                          $"R:\n{rtxt}\n" +
                          $"T: ({M.T.x:F3}, {M.T.y:F3}, {M.T.z:F3})\n" +
                          $"Scale: {M.scale:F2}";
    }

    public void Btn_ToggleView()
    {
        vis.ToggleView();
        // Veri varsa yeniden göster
        if (P != null && Q != null && QAligned != null)
            vis.ShowPoints(P, Q, QAligned);
    }


    public void Btn_DeterministicAlign()
    {
        if (P == null || Q == null || P.Count < 3 || Q.Count < 3)
        {
            resultText.text = "Need at least 3 points in each set.";
            return;
        }

        // basitçe ilk 3 noktayý kullan
        var model = Registration.EstimateRigidFromTriangles(
            P[0], P[1], P[2],
            Q[0], Q[1], Q[2]
        );

        QAligned = Registration.ApplyRigid(Q, model);
        vis.ShowPoints(P, Q, QAligned);

        var R = model.R;
        string rtxt =
            $"[{R[0, 0]:F3} {R[0, 1]:F3} {R[0, 2]:F3}]\n" +
            $"[{R[1, 0]:F3} {R[1, 1]:F3} {R[1, 2]:F3}]\n" +
            $"[{R[2, 0]:F3} {R[2, 1]:F3} {R[2, 2]:F3}]";

        resultText.text = "Method: 3-Point\n" +
                          $"R:\n{rtxt}\n" +
                          $"T: ({model.T.x:F3}, {model.T.y:F3}, {model.T.z:F3})\n" +
                          $"Scale: {model.scale:F2}";
    }

}
