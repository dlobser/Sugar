using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_Layer : MonoBehaviour
    {
        public List<SP_Bulb> bulbs;
        SP_BuildBulbs builder;
        public int bulbAmount;
        public float bulbRadius;

        // Use this for initialization
        public void Init()
        {
            bulbs = new List<SP_Bulb>();
            builder = GetComponent<SP_BuildBulbs>();
			builder.amount = bulbAmount;
			builder.radius = bulbRadius;
            builder.container = this.gameObject;
            bulbs = builder.Build(bulbs);

        }

        public void PingBulb(int which)
        {
            bulbs[which].Ping();
        }


        public SP_Bulb FindClosestBulb(Vector3 v)
        {
            SP_Bulb closestBulb = null;
            float min = 1e6f;

            foreach (SP_Bulb bulb in bulbs)
            {
                float dist = Vector3.Distance(bulb.transform.position, v);
                if (dist < min)
                {
                    min = dist;
                    closestBulb = bulb;
                }
            }
            return closestBulb;
        }


    }
}
