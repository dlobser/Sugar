using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_Stroke : MonoBehaviour
    {

        public List<Vector3> trail { get; set; }
        public int step { get; set; }
        public bool isDrawing { get; set; }
        public bool isPlaying { get; set; }
        public bool isDying { get; set; }
        public bool waitForDeathToRebuild = false;
        public bool readyToDie = false;
        public GameObject container;
        public float maxTime;
        public virtual void Play() { }
        public virtual void Setup() { }
        public virtual void Draw(Vector3 pos) { }
        public virtual void FinishDrawing() { }
        public virtual void OnStart() { }
        public virtual void OnEnd() { }
        public virtual void Animate() { }

    }
}