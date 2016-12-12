//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{
    [DisallowMultipleComponent]
    public abstract class BaseMultiMethodRaycaster : BaseRaycaster
    {
        protected readonly IndexedSet<IRaycastMethod> methods = new IndexedSet<IRaycastMethod>();
#if UNITY_EDITOR
        protected override void Reset()
        {
            base.Reset();
            if (GetComponent<PhysicsRaycastMethod>() == null) { gameObject.AddComponent<PhysicsRaycastMethod>(); }
            if (GetComponent<CanvasRaycastMethod>() == null) { gameObject.AddComponent<CanvasRaycastMethod>(); }
        }
#endif
        public void AddRaycastMethod(IRaycastMethod obj)
        {
            methods.AddUnique(obj);
        }

        public void RemoveRaycastMethod(IRaycastMethod obj)
        {
            methods.Remove(obj);
        }
    }
}