using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP { 
    public class SP_Sequencer : MonoBehaviour {


        public List<SP_Layer> layers = new List<SP_Layer>();
        public SP_Layer layer;
        public int layerAmount;

        float counter = 0;
        float cutTime = 0;
        public float bpm = 1;

        int whichBulb;

        void Start () {
            for (int i = 0; i < layerAmount; i++){
                layers.Add(Instantiate(layer));
                layers[i].Init();
            }
           
	    }
	    
        public void MakeStroke(SP_Stroke stroke ,SP_Brush brush){
            SP_Bulb b = layers[0].FindClosestBulb(brush.transform.position);
            //stroke.container = layers[0].GetComponent<SP_BuildBulbs>().container;
            b.strokes.Add(stroke);
            stroke.transform.parent = b.transform;
        }

        void Update () {
			
            cutTime = 60/bpm;
            counter += Time.deltaTime;

            if(counter>cutTime){
                counter = 0;
                whichBulb++;
                if (whichBulb >= layers[0].bulbs.Count)
                    whichBulb = 0;
                layers[0].PingBulb(whichBulb);
            }

            for (int i = 0; i < layers.Count; i++){
                for (int j = 0; j < layers[i].bulbs.Count; j++){
                    layers[i].bulbs[j].Animate();
                }
            }
        }

    }
}