using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_StrokeRadial : SP_Stroke
    {

        public float counter = 0;
        TrailRenderer tRend;
        public GameObject paint;
        public GameObject canvas;
        GameObject thisPaint;
        List<GameObject> thesePaints;

        GameObject tempContainer;


        public int radialRepeats;


        public override void Setup() {
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
            //thisPaint.transform.localPosition = trail[step];
            if(counter<maxTime)
                counter += Time.deltaTime;
            for (int i = 0; i < thesePaints.Count; i++)
            {
                thesePaints[i].GetComponent<TrailRenderer>().time = counter;
                thesePaints[i].transform.localPosition = pos;
            }
            isDrawing = true;
        }

        public override void FinishDrawing()
        {
            base.FinishDrawing();
            isDrawing = false;
        }

        public override void OnStart() {
            tempContainer = new GameObject();
            tempContainer.name = "Container";
            thesePaints = new List<GameObject>();
            for (int i = 0; i < radialRepeats; i++)
            {
                GameObject g = new GameObject();
               
                thesePaints.Add(Instantiate(paint));
                thesePaints[i].transform.parent = g.transform;
                g.transform.localEulerAngles = new Vector3(0, 0, 360*((float)i / (float)radialRepeats));
                g.transform.parent = tempContainer.transform;
                tRend = thesePaints[i].GetComponent<TrailRenderer>();
                tRend.time = counter;
            }
            //tempContainer.transform.position = container.transform.position;
            //tempContainer.transform.rotation = container.transform.rotation;
            //tempContainer.transform.localScale = container.transform.localScale;
            //thisPaint = Instantiate(paint);
            //thisPaint.transform.parent = canvas.transform;

        }


        public override void OnEnd() {
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

            Destroy(container);
          
        }

        public override void Animate() {
            if (isDrawing || isPlaying)
            {
               
                for (int i = 0; i < thesePaints.Count; i++)
                {
                    if(thesePaints[i]!=null)
                        thesePaints[i].transform.localPosition = trail[Mathf.Min(step, trail.Count - 1)];
                }
             
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