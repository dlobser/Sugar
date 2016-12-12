//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using UnityEngine;

namespace HTC.UnityPlugin.Pointer3D
{
    public abstract class BaseRaySegmentGenerator
    {
        public readonly Transform transform;

        public BaseRaySegmentGenerator(Transform transform)
        {
            this.transform = transform;
        }

        public virtual float velocity { get; set; }

        public virtual Vector3 gravity { get; set; }

        public abstract void Reset();

        public abstract bool NextSegment(out Vector3 direction, out float distance);
    }
}