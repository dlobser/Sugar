//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using UnityEngine;

namespace HTC.UnityPlugin.Pointer3D
{
    public class DefaultRaySegGen : BaseRaySegmentGenerator
    {
        public DefaultRaySegGen(Transform transform) : base(transform) { }

        public override void Reset() { }

        public override bool NextSegment(out Vector3 direction, out float distance)
        {
            direction = transform.forward;
            distance = float.PositiveInfinity;

            return true;
        }
    }
}