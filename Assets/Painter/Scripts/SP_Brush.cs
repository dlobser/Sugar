using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using HTC.UnityPlugin.Vive;


// Attach to Vive controllers

namespace ONSP
{
    public class SP_Brush : MonoBehaviour
    {

        public bool left;
        string whichHand;
        HandRole hand;
        public SP_Stroke[] strokes;
		int whichStroke;
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
			if (ViveInput.GetPress (hand, ControllerButton.Pad)) {
				whichStroke++;
				if (whichStroke >= strokes.Length)
					whichStroke = 0;
			}
            if (ViveInput.GetPress(hand, ControllerButton.Trigger))
            {
                if (!isDrawing)
                {
                    thisStroke = Instantiate(strokes[whichStroke]);
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

       
    }
}