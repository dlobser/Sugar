using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;

namespace ONSP
{
    public class SP_Brush : MonoBehaviour
    {

        public bool left;
        string whichHand;
        HandRole hand;
        public SP_Stroke stroke;
        SP_Stroke thisStroke;
        bool isDrawing;
        public SP_Sequencer sequencer;
      

        // Use this for initialization
        void Start()
        {
            if (left)
            {
                whichHand = "left";
                hand = HandRole.LeftHand;
            }
            else
            {
                whichHand = "right";
                hand = HandRole.RightHand;
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (ViveInput.GetPress(hand, ControllerButton.Trigger))
            {
                if (!isDrawing)
                {
                    thisStroke = Instantiate(stroke);
                    thisStroke.Setup();
                    sequencer.MakeStroke(thisStroke, this);
                    isDrawing = true;
                }
                thisStroke.Draw(this.transform.position);
            }
            else if (ViveInput.GetPressUp(hand, ControllerButton.Trigger))
            {
                thisStroke.OnEnd();
                isDrawing = false;
            }
        }

        void Press(HandRole hand,string message)
        {


        }
    }
}