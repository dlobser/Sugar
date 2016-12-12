//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.Pointer3D
{
    public enum RaycastMode
    {
        DefaultRaycast,
        Projection,
        Projectile,
    }

    public class Pointer3DRaycaster : BaseFallbackCamRaycaster
    {
        public const float MIN_SEGMENT_DISTANCE = 0.01f;

        [NonSerialized]
        private Pointer3DEventData hoverEventData;
        [NonSerialized]
        private ReadOnlyCollection<Pointer3DEventData> buttonEventDataListReadOnly;
        [NonSerialized]
        private ReadOnlyCollection<RaycastResult> sortedRaycastResultsReadOnly;
        [NonSerialized]
        private ReadOnlyCollection<Vector3> breakPointsReadOnly;

        [NonSerialized]
        protected readonly List<Pointer3DEventData> buttonEventDataList = new List<Pointer3DEventData>();
        [NonSerialized]
        protected readonly List<RaycastResult> sortedRaycastResults = new List<RaycastResult>();
        [NonSerialized]
        protected readonly List<Vector3> breakPoints = new List<Vector3>();

        [NonSerialized]
        private BaseRaySegmentGenerator segGen;

        [SerializeField]
        private RaycastMode m_raycastMode = RaycastMode.DefaultRaycast;
        [SerializeField]
        private float m_velocity = 3f;
        [SerializeField]
        private Vector3 m_gravity = Vector3.down;

        public bool showDebugRay = true;

        public Pointer3DEventData HoverEventData
        {
            get { return hoverEventData ?? (hoverEventData = new Pointer3DEventData(EventSystem.current)); }
        }

        public ReadOnlyCollection<Pointer3DEventData> ButtonEventDataList
        {
            get { return buttonEventDataListReadOnly ?? (buttonEventDataListReadOnly = buttonEventDataList.AsReadOnly()); }
        }

        public ReadOnlyCollection<RaycastResult> SortedRaycastResults
        {
            get { return sortedRaycastResultsReadOnly ?? (sortedRaycastResultsReadOnly = sortedRaycastResults.AsReadOnly()); }
        }

        public ReadOnlyCollection<Vector3> BreakPoints
        {
            get { return breakPointsReadOnly ?? (breakPointsReadOnly = breakPoints.AsReadOnly()); }
        }

        public RaycastMode raycastMode
        {
            get { return m_raycastMode; }
            set
            {
                if (m_raycastMode != value)
                {
                    m_raycastMode = value;
                    InitSegmentGenerator();
                }
            }
        }

        public float velocity
        {
            get { return m_velocity; }
            set
            {
                if (m_velocity != value)
                {
                    m_velocity = value;
                    if (segGen != null)
                    {
                        segGen.velocity = value;
                    }
                }
            }
        }

        public Vector3 gravity
        {
            get { return m_gravity; }
            set
            {
                if (m_gravity != value)
                {
                    m_gravity = value;
                    if (segGen != null)
                    {
                        segGen.gravity = value;
                    }
                }
            }
        }

        public virtual Vector2 GetScrollDelta()
        {
            return Vector2.zero;
        }
#if UNITY_EDITOR
        protected override void OnValidate()
        {
            base.OnValidate();

            if (Application.isPlaying)
            {
                InitSegmentGenerator();
                if (segGen != null)
                {
                    segGen.velocity = m_velocity;
                    segGen.gravity = m_gravity;
                }
            }
        }
#endif
        protected override void Start()
        {
            base.Start();
            Pointer3DInputModule.AddRaycaster(this);
        }

        // override OnEnable & OnDisable on purpose so that this BaseRaycaster won't be registered into RaycasterManager
        protected override void OnEnable()
        {
            //base.OnEnable();
        }

        protected override void OnDisable()
        {
            //base.OnDisable();
        }

        protected override void OnDestroy()
        {
            Pointer3DInputModule.RemoveRaycasters(this);
            base.OnDestroy();
        }

        public void InitSegmentGenerator()
        {
            switch (m_raycastMode)
            {
                case RaycastMode.Projection:
                    if (segGen == null || !(segGen is ProjectionRaySegGen)) { segGen = new ProjectionRaySegGen(transform); }
                    break;
                case RaycastMode.Projectile:
                    if (segGen == null || !(segGen is ProjectileRaySegGen)) { segGen = new ProjectileRaySegGen(transform); }
                    break;
                case RaycastMode.DefaultRaycast:
                default:
                    if (segGen == null || !(segGen is DefaultRaySegGen)) { segGen = new DefaultRaySegGen(transform); }
                    break;
            }

            segGen.velocity = m_velocity;
            segGen.gravity = m_gravity;
        }

        // called by Pointer3DInputModule
        public virtual void Raycast()
        {
            if (segGen == null)
            {
                InitSegmentGenerator();
            }

            sortedRaycastResults.Clear();

            breakPoints.Clear();
            segGen.Reset();

            var zScale = transform.lossyScale.z;
            var amountDistance = (FarDistance - NearDistance) * zScale;
            var origin = transform.TransformPoint(0f, 0f, NearDistance);

            breakPoints.Add(origin);

            bool hasNext = true;
            Vector3 direction;
            float distance;
            Ray ray;
            RaycastResult firstHit = default(RaycastResult);

            do
            {
                hasNext = segGen.NextSegment(out direction, out distance);

                if (distance < MIN_SEGMENT_DISTANCE)
                {
                    Debug.LogWarning("RaySegment.distance cannot smaller than " + MIN_SEGMENT_DISTANCE + "! distance=" + distance.ToString("0.000"));
                    break;
                }

                distance *= zScale;

                if (distance < amountDistance)
                {
                    amountDistance -= distance;
                }
                else
                {
                    distance = amountDistance;
                    amountDistance = 0f;
                }
                
                ray = new Ray(origin, direction);

                // move event camera in place
                eventCamera.farClipPlane = eventCamera.nearClipPlane + distance;
                eventCamera.transform.position = ray.origin - (ray.direction * eventCamera.nearClipPlane);
                eventCamera.transform.rotation = Quaternion.LookRotation(ray.direction, transform.up);

                ForeachRaycastMethods(ray, distance, sortedRaycastResults);

                firstHit = FirstRaycastResult();
                // end loop if raycast hit
                if (firstHit.isValid)
                {
                    breakPoints.Add(firstHit.worldPosition);
#if UNITY_EDITOR
                    if (showDebugRay)
                    {
                        Debug.DrawLine(breakPoints[breakPoints.Count - 2], breakPoints[breakPoints.Count - 1], Color.green);
                    }
#endif
                    break;
                }
                // otherwise, shift to next iteration
                origin = ray.GetPoint(distance);
                breakPoints.Add(origin);
#if UNITY_EDITOR
                if (showDebugRay)
                {
                    Debug.DrawLine(breakPoints[breakPoints.Count - 2], breakPoints[breakPoints.Count - 1], Color.red);
                }
#endif
            }
            while (hasNext && amountDistance > 0f);
        }
        // called by StandaloneInputModule, not supported
        public override void Raycast(PointerEventData eventData, List<RaycastResult> resultAppendList) { }

        protected void ForeachRaycastMethods(Ray ray, float distance, List<RaycastResult> resultAppendList)
        {
            var results = ListPool<RaycastResult>.Get();

            for (int i = methods.Count - 1; i >= 0; --i)
            {
                var method = methods[i];
                if (!method.enabled) { continue; }
                method.Raycast(ray, distance, results);
            }

            var comparer = GetRaycasterResultComparer();
            if (comparer != null)
            {
                results.Sort(comparer);
            }

            for (int i = 0, imax = results.Count; i < imax; ++i)
            {
                resultAppendList.Add(results[i]);
            }

            ListPool<RaycastResult>.Release(results);
        }

        protected virtual Comparison<RaycastResult> GetRaycasterResultComparer()
        {
            return Pointer3DInputModule.defaultRaycastComparer;
        }

        public RaycastResult FirstRaycastResult()
        {
            for (int i = 0, imax = sortedRaycastResults.Count; i < imax; ++i)
            {
                if (sortedRaycastResults[i].gameObject == null) { continue; }
                return sortedRaycastResults[i];
            }
            return default(RaycastResult);
        }
    }
}