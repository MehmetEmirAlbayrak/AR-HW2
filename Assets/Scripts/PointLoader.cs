using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public static class PointLoader
{
    public static List<Vector3> LoadFromStreaming(string filename)
    {
        string path = Path.Combine(Application.streamingAssetsPath, filename);
        string[] lines = File.ReadAllLines(path);

        int n = int.Parse(lines[0].Trim());
        float[] xs = lines[1].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s)).ToArray();
        float[] ys = lines[2].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s)).ToArray();
        float[] zs = lines[3].Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries).Select(s => float.Parse(s)).ToArray();

        if (xs.Length != n || ys.Length != n || zs.Length != n)
            throw new Exception("Point file lengths mismatch.");

        var pts = new List<Vector3>(n);
        for (int i = 0; i < n; i++) pts.Add(new Vector3(xs[i], ys[i], zs[i]));
        return pts;
    }
}
