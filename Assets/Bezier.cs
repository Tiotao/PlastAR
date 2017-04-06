using UnityEngine;
using System.Collections.Generic;
[RequireComponent(typeof(LineRenderer))]
    public class Bezier : MonoBehaviour {
    public Transform[] controlPoints;
    public LineRenderer lineRenderer;
    private int curveCount = 0;
    private int layerOrder = 0;
    private int SEGMENT_COUNT = 50;

    private Vector3 lastPt;
    private Vector3 secondLastPt;

    private float t = 1;

    Material _guidingLineMaterial;
    void Start() {
        if (!lineRenderer) {
            lineRenderer = GetComponent<LineRenderer>();
        }
        lineRenderer.sortingLayerID = layerOrder;
        lineRenderer.SetVertexCount(SEGMENT_COUNT*2);
        
    }
    void Update() {
        if (t > 0) {
            t = t - 1f * Time.deltaTime;
        } else {
            t = 1;
        }
        DrawCurve(t);
    }
    void DrawCurve(float offset) {
        lineRenderer.material.SetTextureOffset("_MainTex", new Vector2(offset, 0f));
        lastPt = controlPoints [0].position - controlPoints [0].up * 0.5f + Vector3.left * 0.2f;
        secondLastPt = controlPoints [0].position - controlPoints [0].up * 0.5f + Vector3.left * 0.2f;
        for (int i = 0; i < SEGMENT_COUNT; i++) {

            float t = i / (float)SEGMENT_COUNT;
            Vector3 p0 = controlPoints [0].position - controlPoints [0].up * 0.5f + Vector3.left * 0.2f;
            Vector3 p1 = controlPoints [1].position;
            Vector3 c0 = p0 + Vector3.up * 0.5f + Vector3.Normalize(p1-p0) * 0.5f;
            Vector3 c1 = p1 + Vector3.up * 0.5f;
            Vector3 pixel = CalculateCubicBezierPoint(t, p0, c0, c1, p1);
            Vector3 newSecondLast = lastPt + Vector3.Scale(Vector3.Normalize(lastPt - secondLastPt),new Vector3(0.01f,0.01f,0.01f));
            lineRenderer.SetPosition(2*i,  newSecondLast);
            lineRenderer.SetPosition(2*i+1, pixel);

            lastPt = pixel;
            secondLastPt = newSecondLast;
            
            // lineRenderer.SetVertexCount(((SEGMENT_COUNT) + i));
            // lineRenderer.SetPosition((SEGMENT_COUNT) + (i - 1), pixel);
        }
    }
    Vector3 CalculateCubicBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        float uuu = uu * u;
        float ttt = tt * t;
        Vector3 p = uuu * p0;
        p += 3 * uu * t * p1;
        p += 3 * u * tt * p2;
        p += ttt * p3;
        return p;
    }
}