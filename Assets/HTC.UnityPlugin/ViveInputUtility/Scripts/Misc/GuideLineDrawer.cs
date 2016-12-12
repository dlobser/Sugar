using HTC.UnityPlugin.Pointer3D;
using UnityEngine;

public class GuideLineDrawer : MonoBehaviour
{
    public const float MIN_SEGMENT_LENGTH = 0.01f;

    public Transform origin;
    public bool showOnHitOnly;
    public float segmentLength = 0.1f;

    public Pointer3DRaycaster raycaster;
    public LineRenderer lineRenderer;

    //private static List<Vector3> pointsOnCurve = new List<Vector3>();
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (raycaster == null) { raycaster = GetComponent<Pointer3DRaycaster>(); }
        if (raycaster == null) { raycaster = GetComponentInParent<Pointer3DRaycaster>(); }

        if (lineRenderer == null) { lineRenderer = GetComponentInChildren<LineRenderer>(); }
        if (lineRenderer == null && raycaster != null) { lineRenderer = gameObject.AddComponent<LineRenderer>(); }
        if (lineRenderer != null)
        {
            lineRenderer.SetWidth(0.01f, 0.01f);
        }
    }
#endif
    protected virtual void LateUpdate()
    {
        var result = raycaster.FirstRaycastResult();
        if (showOnHitOnly && !result.isValid)
        {
            lineRenderer.enabled = false;
            return;
        }

        var points = raycaster.BreakPoints;
        var pointCount = points.Count;

        if (pointCount < 2)
        {
            return;
        }

        lineRenderer.enabled = true;
        lineRenderer.useWorldSpace = true;

        var startPoint = origin == null ? points[0] : origin.position;
        var endPoint = points[pointCount - 1];

        if (pointCount == 2)
        {
            lineRenderer.SetVertexCount(2);
            lineRenderer.SetPosition(0, startPoint);
            lineRenderer.SetPosition(1, endPoint);
        }
        else
        {
            var systemY = raycaster.gravity;
            var systemX = endPoint - startPoint;
            var systemZ = default(Vector3);

            Vector3.OrthoNormalize(ref systemY, ref systemX, ref systemZ);

            var forward = Vector3.ProjectOnPlane(origin == null ? points[1] - points[0] : origin.forward, systemZ);
            var hitVector = endPoint - startPoint;
            var accY = raycaster.gravity.magnitude;

            var m = Vector3.Dot(forward, systemY) / Vector3.Dot(forward, systemX);
            var hitX = Vector3.Dot(hitVector, systemX);
            var hitY = Vector3.Dot(hitVector, systemY);
            var v0X = hitX * Mathf.Sqrt(0.5f * accY / (hitY - m * hitX));
            var v0Y = m * v0X;
            var endTime = hitX / v0X;
            var segments = Mathf.Max(Mathf.CeilToInt(endTime / Mathf.Max(segmentLength, MIN_SEGMENT_LENGTH)), 0);

            lineRenderer.SetVertexCount(segments + 1);
            lineRenderer.SetPosition(0, startPoint);
            for (int i = 1, imax = segments; i < imax; ++i)
            {
                var t = i * segmentLength;
                var x = v0X * t;
                var y = v0Y * t + 0.5f * accY * t * t;
                lineRenderer.SetPosition(i, systemX * x + systemY * y + startPoint);
            }
            lineRenderer.SetPosition(segments, endPoint);
        }
    }
}
