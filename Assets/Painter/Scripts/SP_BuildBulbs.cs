using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ONSP
{
    public class SP_BuildBulbs : MonoBehaviour
    {
        public int amount { get; set; }
        public float radius { get; set; }
        public GameObject container;
        GameObject tempContainer;
        public SP_Bulb bulb;

        public List<SP_Bulb> Build(List<SP_Bulb> bulbs)
        {
            tempContainer = new GameObject();
            for (int i = 0; i < amount; i++)
            {
                SP_Bulb b = Instantiate(bulb) as SP_Bulb;
                float rotation = (float)i / (float)amount * 360;
                Vector3 pos = new Vector3(
                    radius * Mathf.Cos(((float)i / (float)amount) * Mathf.PI * 2),
                    radius * Mathf.Sin(((float)i / (float)amount) * Mathf.PI * 2),
                    0);
                b.transform.localPosition = pos;
                b.transform.localEulerAngles = new Vector3(0, 0, rotation);
                b.transform.parent = tempContainer.transform;
                bulbs.Add(b);
            }
            tempContainer.transform.position = container.transform.position;
            tempContainer.transform.rotation = container.transform.rotation;
            tempContainer.transform.localScale = container.transform.localScale;
            for (int i = 0; i < amount; i++)
            {
                tempContainer.transform.GetChild(0).transform.parent = container.transform;
            }
            Destroy(tempContainer);
            return bulbs;
        }
      
    }
}