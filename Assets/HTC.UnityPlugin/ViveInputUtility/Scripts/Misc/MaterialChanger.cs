using HTC.UnityPlugin.ColliderEvent;
using HTC.UnityPlugin.Vive;
using System.Collections.Generic;
using UnityEngine;

public class MaterialChanger : MonoBehaviour
    , IColliderEventHoverEnterHandler
    , IColliderEventHoverExitHandler
    , IColliderEventPressEnterHandler
    , IColliderEventPressExitHandler
{
    private readonly static List<Renderer> s_rederers = new List<Renderer>();

    public Material Normal;
    public Material Heightlight;
    public Material Pressed;

    public ControllerButton heighlightButton = ControllerButton.Trigger;

    private HashSet<ColliderHoverEventData> hovers = new HashSet<ColliderHoverEventData>();
    private HashSet<ColliderButtonEventData> presses = new HashSet<ColliderButtonEventData>();

    public void OnColliderEventHoverEnter(ColliderHoverEventData eventData)
    {
        if (hovers.Add(eventData) && hovers.Count == 1)
        {
            SetMaterial(Heightlight);
        }
    }

    public void OnColliderEventHoverExit(ColliderHoverEventData eventData)
    {
        if (hovers.Remove(eventData) && hovers.Count == 0)
        {
            SetMaterial(Normal);
        }
    }

    public void OnColliderEventPressEnter(ColliderButtonEventData eventData)
    {
        if (eventData.IsViveButton(heighlightButton) && presses.Add(eventData) && presses.Count == 1)
        {
            SetMaterial(Pressed);
        }
    }

    public void OnColliderEventPressExit(ColliderButtonEventData eventData)
    {
        if (presses.Remove(eventData) && presses.Count == 0)
        {
            SetMaterial(Heightlight);
        }
    }

    private void SetMaterial(Material targetMat)
    {
        if (targetMat == null) { return; }
        GetComponentsInChildren(s_rederers);
        if (s_rederers.Count > 0)
        {
            for (int i = s_rederers.Count - 1; i >= 0; --i)
            {
                s_rederers[i].sharedMaterial = targetMat;
            }
            s_rederers.Clear();
        }
    }
}
