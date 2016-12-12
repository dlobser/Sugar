﻿//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using System.Collections.Generic;
using UnityEngine;

namespace HTC.UnityPlugin.PoseTracker
{
    public abstract class BasePoseTracker : MonoBehaviour, IPoseTracker
    {
        private readonly Dictionary<IPoseModifier, LinkedListNode<IPoseModifier>> modifierTable = new Dictionary<IPoseModifier, LinkedListNode<IPoseModifier>>();
        private readonly LinkedList<IPoseModifier> modifiers = new LinkedList<IPoseModifier>();

        public void AddModifier(IPoseModifier obj)
        {
            if (obj == null || modifierTable.ContainsKey(obj)) { return; }

            var node = modifiers.Last;
            if (node == null || node.Value != null)
            {
                node = modifiers.AddFirst(obj);
            }
            else
            {
                modifiers.Remove(node);
                node.Value = obj;
                modifiers.AddFirst(node);
            }

            modifierTable.Add(obj, node);

            // sort new modifier node
            var priorNode = node;
            while (priorNode.Next != null && priorNode.Next.Value != null && priorNode.Next.Value.priority < obj.priority)
            {
                priorNode = priorNode.Next;
            }

            if (priorNode != node)
            {
                modifiers.Remove(node);
                modifiers.AddAfter(priorNode, node);
            }
        }

        public bool RemoveModifier(IPoseModifier obj)
        {
            LinkedListNode<IPoseModifier> node;
            if (!modifierTable.TryGetValue(obj, out node)) { return false; }
            modifierTable.Remove(obj);
            modifiers.Remove(node);
            node.Value = null;
            modifiers.AddLast(node);
            return true;
        }

        protected void TrackPose(Pose pose, Transform origin = null)
        {
            ModifyPose(ref pose, origin);
            Pose.SetPose(transform, pose, origin);
        }

        private void ModifyPose(ref Pose pose, Transform origin)
        {
            for (var node = modifiers.First; node != null && node.Value != null; node = node.Next)
            {
                if (!node.Value.enabled) { continue; }
                node.Value.ModifyPose(ref pose, origin);
            }
        }
    }
}