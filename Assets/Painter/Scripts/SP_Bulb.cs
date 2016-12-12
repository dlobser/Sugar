using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_Bulb : MonoBehaviour
    {

        public List<SP_Stroke> strokes;
		public GameObject indicator;
        // Use this for initialization

          
        void Start()
        {
            strokes = new List<SP_Stroke>();

        }

        public void Ping()
        {
			if(indicator!=null)
				if (indicator.GetComponent<SP_Ping> () != null)
					indicator.GetComponent<SP_Ping> ().Ping ();
				
            for (int i = 0; i < strokes.Count; i++)
            {
                strokes[i].Play();
            }
      
        }

        public void Animate()
        {
            for (int i = 0; i < strokes.Count; i++)
            {
                strokes[i].Animate();
            }
        }

        IEnumerator PlayStrokes()
        {
            yield return null;
        }
        // Update is called once per frame
        void Update()
        {

        }
    }
}
