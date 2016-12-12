//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using UnityEngine;

namespace HTC.UnityPlugin.Pointer3D
{
    public class ProjectionRaySegGen : BaseRaySegmentGenerator
    {
        public override float velocity { get; set; }
        public override Vector3 gravity { get; set; }

        private bool isFirstSegment = true;

        public ProjectionRaySegGen(Transform transform) : base(transform) { }

        public override void Reset()
        {
            isFirstSegment = true;
        }

        public override bool NextSegment(out Vector3 direction, out float distance)
        {
            if (isFirstSegment && velocity > Pointer3DRaycaster.MIN_SEGMENT_DISTANCE)
            {
                isFirstSegment = false;
                direction = transform.forward;
                distance = velocity;
            }
            else
            {
                direction = gravity;
                distance = float.PositiveInfinity;
            }

            return true;
        }
    }
}