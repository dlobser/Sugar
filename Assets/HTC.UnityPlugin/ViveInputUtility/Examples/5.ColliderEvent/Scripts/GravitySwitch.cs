using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

public class GravitySwitch : MonoBehaviour
    , IColliderEventHoverEnterHandler
{
    public Rigidbody effectTarget;
    public Transform switchObject;

    [SerializeField]
    private bool useGravity = true;
    public Vector3 impalse = Vector3.up;

    private void Start()
    {
        SetUseGravityState(useGravity);
    }

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        useGravity = !useGravity;
        SetUseGravityState(useGravity);
    }

    private void SetUseGravityState(bool value)
    {
        effectTarget.useGravity = value;
        switchObject.eulerAngles = new Vector3(0f, 0f, value ? 15f : -15f);
        if (!value)
        {
            effectTarget.AddForce(impalse, ForceMode.VelocityChange);
        }
    }
}
