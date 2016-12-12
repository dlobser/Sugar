using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_PingColor : SP_Ping
    {

        float count;
        public float highValue;

        public override void Init() { }
        public override void Ping() { count = time; StartCoroutine(Fade()); }

        IEnumerator Fade()
        {
            while (count > 0)
            {
                count -= Time.deltaTime;
                GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1, count * highValue);
                yield return new WaitForSeconds(Time.deltaTime);
            }

        }

    }
}