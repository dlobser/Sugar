//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using System;
using HTC.UnityPlugin.ColliderEvent;
using UnityEngine;

namespace HTC.UnityPlugin.Vive
{
    public static class ViveColliderEventDataExtension
    {
        public static bool IsViveButton(this ColliderButtonEventData eventData, ControllerButton button)
        {
            var viveEvent = eventData as ViveColliderButtonEventData;

            if (viveEvent == null)
            {
                return false;
            }
            else
            {
                return viveEvent.viveButton == button;
            }
        }

        public static bool IsViveButton(this ColliderButtonEventData eventData, HandRole hand, ControllerButton button)
        {
            if (eventData == null) { return false; }

            var viveEvent = eventData as ViveColliderButtonEventData;
            if (viveEvent == null)
            {
                return false;
            }
            else
            {
                return viveEvent.hand == hand && viveEvent.viveButton == button;
            }
        }
    }

    public class ViveColliderButtonEventData : ColliderButtonEventData
    {
        public HandRole hand;
        public ControllerButton viveButton;

        public ViveColliderButtonEventData(IColliderEventCaster eventCaster, HandRole hand, ControllerButton viveButton, int buttonId = 0) : base(eventCaster, buttonId)
        {
            this.hand = hand;
            this.viveButton = viveButton;
        }

        public override bool GetPress() { return ViveInput.GetPress(hand, viveButton); }

        public override bool GetPressDown() { return ViveInput.GetPressDown(hand, viveButton); }

        public override bool GetPressUp() { return ViveInput.GetPressUp(hand, viveButton); }
    }

    public class ViveColliderTriggerValueEventData : ColliderAxisEventData
    {
        public HandRole hand;

        public ViveColliderTriggerValueEventData(IColliderEventCaster eventCaster, HandRole hand, int axisId = 0) : base(eventCaster, Dim.d1, axisId)
        {
            this.hand = hand;
        }

        public override bool IsValueChangedThisFrame()
        {
            xRaw = ViveInput.GetTriggerValue(hand, false) - ViveInput.GetTriggerValue(hand, true);
            return !Mathf.Approximately(xRaw, 0f);
        }

        public float GetCurrentValue()
        {
            return ViveInput.GetTriggerValue(hand);
        }
    }

    public class ViveColliderPadAxisEventData : ColliderAxisEventData
    {
        public HandRole hand;

        public ViveColliderPadAxisEventData(IColliderEventCaster eventCaster, HandRole hand, int axisId = 0) : base(eventCaster, Dim.d2, axisId)
        {
            this.hand = hand;
        }

        public override bool IsValueChangedThisFrame()
        {
            v2 = ViveInput.GetPadTouchDelta(hand);
            return !Mathf.Approximately(v2.sqrMagnitude, 0f);
        }

        public Vector2 GetCurrentValue()
        {
            return ViveInput.GetPadTouchAxis(hand);
        }
    }
}