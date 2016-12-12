﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_StrokeDefault : SP_Stroke
    {

        public float counter = 0;
        TrailRenderer tRend;
        public GameObject paint;
        public GameObject canvas;
        GameObject thisPaint;


        public override void Setup()
        {
            trail = new List<Vector3>();
            OnStart();
        }

        public override void Play()
        {
            if (!isDrawing && !isDying)
            {
                step = 0;
                OnStart();
                isDrawing = true;
            }
        }

        public override void Draw(Vector3 pos)
        {
            trail.Add(pos);
            thisPaint.transform.localPosition = trail[step];
            if (counter < maxTime)
                counter += Time.deltaTime;
            tRend.time = counter;
            isDrawing = true;
        }

        public override void FinishDrawing()
        {
            base.FinishDrawing();
            isDrawing = false;
        }

        public override void OnStart()
        {
            thisPaint = Instantiate(paint);
            thisPaint.transform.parent = canvas.transform;
            tRend = thisPaint.GetComponent<TrailRenderer>();
            tRend.time = counter;
        }


        public override void OnEnd()
        {
            StartCoroutine(WaitToDie());
        }

        IEnumerator WaitToDie()
        {
            float count = 0;
            while (count < counter)
            {
                count += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
            isDying = false;
            Destroy(thisPaint.gameObject);
        }

        public override void Animate()
        {
            if (isDrawing || isPlaying)
            {
               
                if(thisPaint!=null)
                    thisPaint.transform.localPosition = trail[Mathf.Max(0,Mathf.Min(step, trail.Count - 1))];
                if (step > trail.Count)
                {
                    readyToDie = true;
                    isDrawing = false;
                    step = 0;
                }
                step++;
            }
            if (readyToDie)
            {
                OnEnd();
                isDying = true;
                readyToDie = false;
            }
        }

    }
}