//========= Copyright 2016, HTC Corporation. All rights reserved. ===========

using HTC.UnityPlugin.Utility;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.EventSystems;

namespace HTC.UnityPlugin.ColliderEvent
{
    public interface IColliderEventCaster
    {
        GameObject gameObject { get; }
        Transform transform { get; }
        Rigidbody rigid { get; }
        ColliderHoverEventData HoverEventData { get; }
        ReadOnlyCollection<ColliderButtonEventData> ButtonEventDataList { get; }
        ReadOnlyCollection<Collider> HoveredColliders { get; }
    }

    [RequireComponent(typeof(Rigidbody))]
    public class ColliderEventCaster : MonoBehaviour, IColliderEventCaster
    {
        private static HashSet<int> s_gos = new HashSet<int>();

        [NonSerialized]
        private Rigidbody m_rigid;
        [NonSerialized]
        private ColliderHoverEventData hoverEventData;
        [NonSerialized]
        private ReadOnlyCollection<Collider> hoveredCollidersReadOnly;

        [NonSerialized]
        private ReadOnlyCollection<ColliderButtonEventData> buttonEventDataListReadOnly;
        [NonSerialized]
        private ReadOnlyCollection<ColliderAxisEventData> axisEventDataListReadOnly;

        [NonSerialized]
        protected readonly List<ColliderButtonEventData> buttonEventDataList = new List<ColliderButtonEventData>();
        [NonSerialized]
        protected readonly List<ColliderAxisEventData> axisEventDataList = new List<ColliderAxisEventData>();

        private IndexedSet<Collider> hoveredColliders = new IndexedSet<Collider>();
        private IndexedSet<GameObject> hoveredObjects = new IndexedSet<GameObject>();

        public Rigidbody rigid
        {
            get { return m_rigid ?? (m_rigid = GetComponent<Rigidbody>()); }
        }

        public ColliderHoverEventData HoverEventData
        {
            get { return hoverEventData ?? (hoverEventData = new ColliderHoverEventData(this)); }
        }

        public ReadOnlyCollection<ColliderButtonEventData> ButtonEventDataList
        {
            get { return buttonEventDataListReadOnly ?? (buttonEventDataListReadOnly = buttonEventDataList.AsReadOnly()); }
        }

        public ReadOnlyCollection<ColliderAxisEventData> AxisEventDataList
        {
            get { return axisEventDataListReadOnly ?? (axisEventDataListReadOnly = axisEventDataList.AsReadOnly()); }
        }

        public ReadOnlyCollection<Collider> HoveredColliders
        {
            get { return hoveredCollidersReadOnly ?? (hoveredCollidersReadOnly = hoveredColliders.AsReadOnly()); }
        }

        protected virtual void OnTriggerEnter(Collider collider)
        {
            hoveredColliders.AddUnique(collider);
        }

        protected virtual void OnTriggerExit(Collider collider)
        {
            hoveredColliders.Remove(collider);
        }

        protected virtual void Update()
        {
            // remove inactive or disabled collider
            // because OnTriggerExit doesn't automatically called on disabled
            hoveredColliders.RemoveAll((c) => c == null || !c.enabled || c.gameObject == null || !c.gameObject.activeInHierarchy);

            // process enter & stay
            var hoveredObjectsPrev = hoveredObjects;
            hoveredObjects = IndexedSetPool<GameObject>.Get();

            for (int i = hoveredColliders.Count - 1; i >= 0; --i)
            {
                for (var tr = hoveredColliders[i].transform; tr != null; tr = tr.parent)
                {
                    var go = tr.gameObject;

                    if (!hoveredObjects.AddUnique(go)) { break; }

                    if (!hoveredObjectsPrev.Remove(go))
                    {
                        ExecuteEvents.Execute(go, HoverEventData, ExecuteColliderEvents.HoverEnterHandler);
                    }
                }
            }

            // process button events
            for (int i = 0, imax = buttonEventDataList.Count; i < imax; ++i)
            {
                var eventData = buttonEventDataList[i];

                // process button press
                if (eventData.GetPressDown())
                {
                    ProcessPressDown(eventData);
                }
                else if (eventData.GetPress())
                {
                    ProcessPressing(eventData);
                }
                else if (eventData.GetPressUp())
                {
                    ProcessPressUp(eventData);
                }

                // process pressed button enter/exit
                if (eventData.GetPress())
                {
                    var pressEnteredObjectsPrev = eventData.pressEnteredObjects;
                    eventData.pressEnteredObjects = IndexedSetPool<GameObject>.Get();

                    for (int j = hoveredObjects.Count - 1; j >= 0; --j)
                    {
                        var go = hoveredObjects[j];

                        if (!eventData.pressEnteredObjects.AddUnique(go)) { continue; }

                        if (!pressEnteredObjectsPrev.Remove(go))
                        {
                            ExecuteEvents.Execute(go, eventData, ExecuteColliderEvents.PressEnterHandler);
                        }
                    }

                    for (int j = pressEnteredObjectsPrev.Count - 1; j >= 0; --j)
                    {
                        ExecuteEvents.Execute(pressEnteredObjectsPrev[j], eventData, ExecuteColliderEvents.PressExitHandler);
                    }

                    IndexedSetPool<GameObject>.Release(pressEnteredObjectsPrev);
                }
                else
                {
                    for (int j = eventData.pressEnteredObjects.Count - 1; j >= 0; --j)
                    {
                        ExecuteEvents.Execute(eventData.pressEnteredObjects[j], eventData, ExecuteColliderEvents.PressExitHandler);
                    }

                    eventData.pressEnteredObjects.Clear();
                }
            }

            // process axis events
            for (int i = 0, imax = axisEventDataList.Count; i < imax; ++i)
            {
                var eventData = axisEventDataList[i];

                if (!eventData.IsValueChangedThisFrame()) { continue; }

                for (int j = hoveredColliders.Count - 1; j >= 0; --j)
                {
                    var handler = ExecuteEvents.GetEventHandler<IColliderEventAxisChangedHandler>(hoveredColliders[j].gameObject);

                    if (handler == null) { continue; }

                    if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.AxisChangedHandler);
                }
            }
            s_gos.Clear();

            // process leave
            // now stayingObjectsPrev left with handlers that are exited
            for (int i = hoveredObjectsPrev.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(hoveredObjectsPrev[i], HoverEventData, ExecuteColliderEvents.HoverExitHandler);
            }

            IndexedSetPool<GameObject>.Release(hoveredObjectsPrev);
        }

        protected void ProcessPressDown(ColliderButtonEventData eventData)
        {
            // press down
            for (int j = hoveredColliders.Count - 1; j >= 0; --j)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventPressDownHandler>(hoveredColliders[j].gameObject);

                if (handler == null) { continue; }

                if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.PressDownHandler);
            }
            s_gos.Clear();
            // drag start
            for (int j = hoveredColliders.Count - 1; j >= 0; --j)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventDragStartHandler>(hoveredColliders[j].gameObject);

                if (handler == null) { continue; }

                if (!eventData.draggingHandlers.AddUnique(handler)) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.DragStartHandler);

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.DragUpdateHandler);
            }
            // click start
            for (int j = hoveredColliders.Count - 1; j >= 0; --j)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventClickHandler>(hoveredColliders[j].gameObject);

                if (handler == null) { continue; }

                eventData.clickingHandlers.AddUnique(handler);
            }
        }

        protected void ProcessPressing(ColliderButtonEventData eventData)
        {
            // dragging
            for (int j = eventData.draggingHandlers.Count - 1; j >= 0; --j)
            {
                ExecuteEvents.Execute(eventData.draggingHandlers[j], eventData, ExecuteColliderEvents.DragUpdateHandler);
            }
        }

        protected void ProcessPressUp(ColliderButtonEventData eventData)
        {
            // press up
            for (int j = hoveredColliders.Count - 1; j >= 0; --j)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventPressUpHandler>(hoveredColliders[j].gameObject);

                if (handler == null) { continue; }

                if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.PressUpHandler);
            }
            s_gos.Clear();
            // drag end
            for (int j = eventData.draggingHandlers.Count - 1; j >= 0; --j)
            {
                ExecuteEvents.Execute(eventData.draggingHandlers[j], eventData, ExecuteColliderEvents.DragEndHandler);
            }
            // drop (execute if only there are dragging handlers)
            if (eventData.draggingHandlers.Count > 0 && hoveredColliders.Count > 0)
            {
                for (int j = hoveredColliders.Count - 1; j >= 0; --j)
                {
                    var handler = ExecuteEvents.GetEventHandler<IColliderEventDropHandler>(hoveredColliders[j].gameObject);

                    if (handler == null) { continue; }

                    if (!s_gos.Add(handler.GetInstanceID())) { continue; }

                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.DropHandler);
                }
            }
            s_gos.Clear();
            // click end (execute only if pressDown handler and pressUp handler are the same)
            for (int j = hoveredColliders.Count - 1; j >= 0; --j)
            {
                var handler = ExecuteEvents.GetEventHandler<IColliderEventClickHandler>(hoveredColliders[j].gameObject);

                if (handler == null) { continue; }

                if (eventData.clickingHandlers.Remove(handler))
                {
                    ExecuteEvents.Execute(handler, eventData, ExecuteColliderEvents.ClickHandler);
                }
            }

            eventData.clickingHandlers.Clear();
            eventData.draggingHandlers.Clear();
        }

        protected virtual void OnDisable()
        {
            hoveredColliders.RemoveAll((c) => c == null || !c.enabled || c.gameObject == null || !c.gameObject.activeInHierarchy);

            // release all
            for (int i = 0, imax = buttonEventDataList.Count; i < imax; ++i)
            {
                var eventData = buttonEventDataList[i];

                ProcessPressUp(eventData);

                for (int j = eventData.pressEnteredObjects.Count - 1; j >= 0; --j)
                {
                    ExecuteEvents.Execute(eventData.pressEnteredObjects[j], eventData, ExecuteColliderEvents.PressExitHandler);
                }

                eventData.clickingHandlers.Clear();
                eventData.draggingHandlers.Clear();
                eventData.pressEnteredObjects.Clear();
            }

            for (int i = hoveredObjects.Count - 1; i >= 0; --i)
            {
                ExecuteEvents.Execute(hoveredObjects[i], hoverEventData, ExecuteColliderEvents.HoverExitHandler);
            }

            hoveredColliders.Clear();
            hoveredObjects.Clear();
        }
    }
}