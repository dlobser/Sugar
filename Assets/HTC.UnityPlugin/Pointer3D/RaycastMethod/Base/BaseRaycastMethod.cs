//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{
    public abstract class BaseRaycastMethod : MonoBehaviour, IRaycastMethod
    {
        private Pointer3DRaycaster m_raycaster;
        public Pointer3DRaycaster raycaster { get { return m_raycaster ?? (m_raycaster = GetComponent<Pointer3DRaycaster>()); } }

        protected virtual void Start()
        {
            raycaster.AddRaycastMethod(this);
        }

        protected virtual void OnEnable() { }

        protected virtual void OnDisable() { }

        protected virtual void OnDestroy()
        {
            raycaster.RemoveRaycastMethod(this);
        }

        public abstract void Raycast(Ray ray, float distance, List<RaycastResult> raycastResults);
    }
}