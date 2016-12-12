using HTC.UnityPlugin.Pointer3D;
using UnityEngine;

public class ReticlePoser : MonoBehaviour
{
    public Pointer3DRaycaster raycaster;
    public Transform target;
    public bool showOnHitOnly = true;

    public GameObject hitTarget;
    public float hitDistance;
#if UNITY_EDITOR
    protected virtual void Reset()
    {
        if (raycaster == null) { raycaster = GetComponent<Pointer3DRaycaster>(); }
        if (raycaster == null) { raycaster = GetComponentInParent<Pointer3DRaycaster>(); }
    }
#endif
    protected virtual void LateUpdate()
    {
        var points = raycaster.BreakPoints;
        var pointCount = points.Count;
        var result = raycaster.FirstRaycastResult();
        if ((showOnHitOnly && !result.isValid) || pointCount <= 1)
        {
            target.gameObject.SetActive(false);
            return;
        }
        
        target.gameObject.SetActive(true);

        if (result.isValid)
        {
            target.position = result.worldPosition;
            target.rotation = Quaternion.LookRotation(result.worldNormal, raycaster.transform.forward);

            hitTarget = result.gameObject;
            hitDistance = result.distance;
        }
        else
        {
            target.position = points[pointCount - 1];
            target.rotation = Quaternion.LookRotation(points[pointCount - 1] - points[pointCount - 2], raycaster.transform.forward);

            hitTarget = null;
            hitDistance = 0f;
        }
    }
}
