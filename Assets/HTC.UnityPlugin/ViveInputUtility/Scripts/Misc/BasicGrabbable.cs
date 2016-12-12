using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Vive;
using HTC.UnityPlugin.Utility;
using UnityEngine;

public class BasicGrabbable : MonoBehaviour
    , IColliderEventDragStartHandler
    , IColliderEventDragUpdateHandler
    , IColliderEventDragEndHandler
{
    public bool alignPosition;
    public bool alignRotation;

    [SerializeField]
    private ControllerButton m_grabButton = ControllerButton.Trigger;

    private Vector3 grabOffsetPos;
    private Quaternion grabOffsetRot;
    private Vector3 lastPos;
    private Quaternion lastRot;
    private Vector3 currPos;
    private Quaternion currRot;
    private float lastDeltaTime;

    private ColliderButtonEventData dragEvent;

    public ControllerButton grabButton
    {
        get
        {
            return m_grabButton;
        }
        set
        {
            m_grabButton = value;
            // set all child MaterialChanger heighlightButton to value;
            var changers = ListPool<MaterialChanger>.Get();
            GetComponentsInChildren(changers);
            for (int i = changers.Count - 1; i >= 0; --i) { changers[i].heighlightButton = value; }
            ListPool<MaterialChanger>.Release(changers);
        }
    
    }

    public bool IsGrabbed { get { return dragEvent != null; } }
#if UNITY_EDITOR
    protected virtual void OnValidate()
    {
        grabButton = m_grabButton;
    }
#endif
    public void OnColliderEventDragStart(ColliderButtonEventData eventData)
    {
        if (!eventData.IsViveButton(m_grabButton)) { return; }

        dragEvent = eventData;
        Debug.Log("boosh");

        grabOffsetPos = alignPosition ? Vector3.zero : eventData.eventCaster.transform.InverseTransformPoint(transform.position);
        grabOffsetRot = alignRotation ? Quaternion.identity : Quaternion.Inverse(eventData.eventCaster.transform.rotation) * transform.rotation;
    }

    public void OnColliderEventDragUpdate(ColliderButtonEventData eventData)
    {
        if (eventData != dragEvent) { return; }

        lastPos = currPos;
        lastRot = currRot;
        lastDeltaTime = Time.deltaTime;

        currPos = eventData.eventCaster.transform.TransformPoint(grabOffsetPos);
        currRot = eventData.eventCaster.transform.rotation * grabOffsetRot;

        transform.position = currPos;
        transform.rotation = currRot;
    }

    public void OnColliderEventDragEnd(ColliderButtonEventData eventData)
    {
        if (eventData != dragEvent) { return; }

        dragEvent = null;

        transform.position = currPos;
        transform.rotation = currRot;

        var rigid = GetComponent<Rigidbody>();
        if (rigid != null)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;

            rigid.AddForce((currPos - lastPos) / lastDeltaTime, ForceMode.VelocityChange);
            rigid.AddTorque(Vector3.Cross(lastRot * Vector3.forward, currRot * Vector3.forward) / lastDeltaTime, ForceMode.VelocityChange);
        }
    }
}
