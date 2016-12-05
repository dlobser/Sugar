#pragma warning disable

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TREESharp{
	public class TreeTransform : MonoBehaviour {


		List<Dictionary<string, float>> Transforms = new List<Dictionary<string, float>>();
		List<List<int[]>> SelectedJoints = new List<List<int[]>>();
		GameObject root;
		List<List<Vector3>> initialRotation = new List<List<Vector3>>();

		Vector3 sinRotate = Vector3.zero;
		Vector3 sinScale = Vector3.zero;
		Vector3 noiseRotate = Vector3.zero;
		Vector3 rotateOffset = Vector3.zero;
		Vector3 rotate = Vector3.zero;
		Vector3 scale = Vector3.one;

		bool defaultsMade = false;

		void makeDefaults(int amount){
			Transforms.Clear ();
			for (int i = 0; i < amount; i++) {
				Transforms.Add (new Dictionary<string,float> ());
				Transforms[i].Add ("rx", 0);
				Transforms[i].Add ("ry", 0);
				Transforms[i].Add ("rz", 0);

				//offset rotation
				Transforms[i].Add ("orx", 0);
				Transforms[i].Add ("ory", 0);
				Transforms[i].Add ("orz", 0);



				//sin mult
				Transforms[i].Add ("sMult", 0);
				//sin offset axial
				Transforms[i].Add ("saorx", 0);
				Transforms[i].Add ("saory", 0);
				Transforms[i].Add ("saorz", 0);

				Transforms[i].Add ("srorx", 0);
				Transforms[i].Add ("srory", 0);
				Transforms[i].Add ("srorz", 0);
				//sin offset
				Transforms[i].Add ("sorx", 0);
				Transforms[i].Add ("sory", 0);
				Transforms[i].Add ("sorz", 0);
				//sin frequency
				Transforms[i].Add ("sfrx", 0);
				Transforms[i].Add ("sfry", 0);
				Transforms[i].Add ("sfrz", 0);
				//sin speed
				Transforms[i].Add ("ssrx", 0);
				Transforms[i].Add ("ssry", 0);
				Transforms[i].Add ("ssrz", 0);
				//sin multiply
				Transforms[i].Add ("smrx", 0);
				Transforms[i].Add ("smry", 0);
				Transforms[i].Add ("smrz", 0);

				//sin uniform scale (true/false 1/0)
				Transforms[i].Add ("sus", 0);

				Transforms[i].Add ("scale", 1);
				//scale xyz
				Transforms[i].Add ("sx", 0);
				Transforms[i].Add ("sy", 0);
				Transforms[i].Add ("sz", 0);
				//sin scale mult
				Transforms[i].Add ("ssMult", 0);
				//sin offset from root offset
				Transforms[i].Add ("srosx", 0);
				Transforms[i].Add ("srosy", 0);
				Transforms[i].Add ("srosz", 0);
				//sin offset		  s
				Transforms[i].Add ("sosx", 0);
				Transforms[i].Add ("sosy", 0);
				Transforms[i].Add ("sosz", 0);
				//sin frequency	  	s
				Transforms[i].Add ("sfsx", 0);
				Transforms[i].Add ("sfsy", 0);
				Transforms[i].Add ("sfsz", 0);
				//sin speed		  	s
				Transforms[i].Add ("sssx", 0);
				Transforms[i].Add ("sssy", 0);
				Transforms[i].Add ("sssz", 0);
				//sin multiply	      s
				Transforms[i].Add ("smsx", 0);
				Transforms[i].Add ("smsy", 0);
				Transforms[i].Add ("smsz", 0);

				//noise joint multiply
				Transforms[i].Add ("nMult", 0);
				//noise root offset
				Transforms[i].Add ("nrorx", 0);
				Transforms[i].Add ("nrory", 0);
				Transforms[i].Add ("nrorz", 0);
				//noise offset axial
				Transforms[i].Add ("naorx", 0);
				Transforms[i].Add ("naory", 0);
				Transforms[i].Add ("naorz", 0);

				//noise offset
				Transforms[i].Add ("norx", 0);
				Transforms[i].Add ("nory", 0);
				Transforms[i].Add ("norz", 0);
				//noise frequency	 
				Transforms[i].Add ("nfrx", 0);
				Transforms[i].Add ("nfry", 0);
				Transforms[i].Add ("nfrz", 0);
				//noise speed		 
				Transforms[i].Add ("nsrx", 0);
				Transforms[i].Add ("nsry", 0);
				Transforms[i].Add ("nsrz", 0);
				//noise multiply	  
				Transforms[i].Add ("nmrx", 0);
				Transforms[i].Add ("nmry", 0);
				Transforms[i].Add ("nmrz", 0);

				Transforms[i].Add ("length", 0);

				//length mult
				Transforms[i].Add ("lMult", 0);
				//sin offset length
				Transforms[i].Add ("sol", 0);
				//sin frequency length
				Transforms[i].Add ("sfl", 0);
				//sin speed length
				Transforms[i].Add ("ssl", 0);
				//sin axis offset length
				Transforms[i].Add ("saol", 0);
				//sin root offset length
				Transforms[i].Add ("srol", 0);
				//sin multiply length
				Transforms[i].Add ("sml", 0);

				//length color
				Transforms[i].Add ("cMult", 0);
				//sin offset color
				Transforms[i].Add ("soc", 0);
				//sin frequency color
				Transforms[i].Add ("sfc", 0);
				//sin speed color
				Transforms[i].Add ("ssc", 0);
				//sin axis offset color
				Transforms[i].Add ("saoc", 0);
				//sin root offset color
				Transforms[i].Add ("sroc", 0);
				//sin multiply color
				Transforms[i].Add ("smc", 0);


				//colors
				Transforms[i].Add ("cr", 0);
				Transforms[i].Add ("cg", 0);
				Transforms[i].Add ("cb", 0);
			}
		}
				
		public void Setup(string[] joints, string[] args, TREE tree){

	//		if (!defaultsMade) {
			if (joints != null) {
				makeDefaults (joints.Length);
				root = tree.gameObject;
				defaultsMade = true;
				//		}

				initialRotation.Clear ();
				SelectedJoints.Clear ();

				for (int i = 0; i < joints.Length; i++) {

					List<int[]> firstList = TREEUtils.makeList (joints [i], tree.GetComponent<TREE> ());
					List<Vector3> initRotations = new List<Vector3> ();

					for (int p = 0; p < firstList.Count; p++) {
						GameObject g = TREEUtils.findJoint (firstList [p], 0, root.transform.GetChild (0).gameObject);
						initRotations.Add (g.transform.localEulerAngles);
					}

					initialRotation.Add (initRotations);	
					SelectedJoints.Add (firstList);

					string[] arg = args [i].Split (new string[] { "," }, System.StringSplitOptions.None);
					for (int j = 0; j < arg.Length; j++) {
						string[] a = arg [j].Split (new string[] { ":" }, System.StringSplitOptions.None);
						if (a.Length > 1)
							Transforms [i] [a [0]] = float.Parse (a [1]);
					}
				}
			}
		}

		public void updateValue(int which, string control, float value){
			Transforms [which] [control] = value;
		}

		public void returnToInitialState(){
			for (int i = 0; i < SelectedJoints.Count; i++) {
				for (int j = 0; j < SelectedJoints [i].Count; j++) {
					GameObject g;
					if(root.GetComponent<TREE>().jointDictionary.Count>0 && root.GetComponent<TREE>().jointDictionary.ContainsKey(TREEUtils.arrayToString(SelectedJoints[i][j])))
						g = root.GetComponent<TREE>().jointDictionary[TREEUtils.arrayToString(SelectedJoints[i][j])].gameObject;
					else
						g = TREEUtils.findJoint (SelectedJoints [i] [j], 0, root.transform.GetChild (0).gameObject);
					if(g!=null)
						g.transform.localEulerAngles = initialRotation [i] [j];

				}
			}
		}

		public void Animate(float timer){
			for (int i = 0; i < SelectedJoints.Count; i++) {
				for (int j = 0; j < SelectedJoints [i].Count; j++) {
					
					GameObject g;
					if(root.GetComponent<TREE>().jointDictionary.Count>0 && root.GetComponent<TREE>().jointDictionary.ContainsKey(TREEUtils.arrayToString(SelectedJoints[i][j])))
						g = root.GetComponent<TREE>().jointDictionary[TREEUtils.arrayToString(SelectedJoints[i][j])].gameObject;
					else
						g = TREEUtils.findJoint (SelectedJoints [i] [j], 0, root.transform.GetChild (0).gameObject);

					if(g!=null){
						int jointNumber = g.GetComponent<Joint> ().joint;
						int jointOffset = g.GetComponent<Joint> ().offset;
						int jointOffset2 = g.GetComponent<Joint> ().offset2;
						Vector3 init = initialRotation [i] [j];

						sinRotate = Vector3.zero;
		             	noiseRotate = Vector3.zero;
						rotateOffset = Vector3.zero;
						sinScale = Vector3.zero;

						rotate.Set(
							Transforms[i]["rx"],
							Transforms[i]["ry"],
							Transforms[i]["rz"]
						);
							

						if(Transforms [i] ["smrx"]!=0||Transforms [i] ["smry"]!=0||Transforms [i] ["smrz"]!=0)
							sinRotate.Set(
								((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smrx"]) * Mathf.Sin (((Transforms [i] ["ssrx"] * timer + Transforms [i] ["sorx"] + (Transforms [i] ["srorx"]*jointOffset) +  (Transforms [i] ["saorx"]*jointOffset2)) + (Transforms [i] ["sfrx"] * jointNumber))),
								((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smry"]) * Mathf.Sin (((Transforms [i] ["ssry"] * timer + Transforms [i] ["sory"] + (Transforms [i] ["srory"]*jointOffset) +  (Transforms [i] ["saory"]*jointOffset2)) + (Transforms [i] ["sfry"] * jointNumber))),
								((Transforms [i] ["sMult"]*jointNumber)+Transforms [i] ["smrz"]) * Mathf.Sin (((Transforms [i] ["ssrz"] * timer + Transforms [i] ["sorz"] + (Transforms [i] ["srorz"]*jointOffset) +  (Transforms [i] ["saorz"]*jointOffset2)) + (Transforms [i] ["sfrz"] * jointNumber)))
							);
						if(Transforms [i] ["nmrx"]!=0||Transforms [i] ["nmry"]!=0||Transforms [i] ["nmrz"]!=0)
							noiseRotate.Set (
								((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmrx"]) * TREEUtils.Noise (((Transforms [i] ["nsrx"] * -timer + Transforms [i] ["norx"] +  (Transforms [i] ["nrorx"]*jointOffset) + (Transforms [i] ["naorx"]*jointOffset2)) + (Transforms [i] ["nfrx"] * jointNumber))),
								((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmry"]) * TREEUtils.Noise (((Transforms [i] ["nsry"] * -timer + Transforms [i] ["nory"] +  (Transforms [i] ["nrory"]*jointOffset) + (Transforms [i] ["naory"]*jointOffset2)) + (Transforms [i] ["nfry"] * jointNumber))),
								((Transforms [i] ["nMult"]*jointNumber)+Transforms [i] ["nmrz"]) * TREEUtils.Noise (((Transforms [i] ["nsrz"] * -timer + Transforms [i] ["norz"] +  (Transforms [i] ["nrorz"]*jointOffset) + (Transforms [i] ["naorz"]*jointOffset2)) + (Transforms [i] ["nfrz"] * jointNumber)))
							);
						
					
						if(Transforms [i] ["smsx"]!=0||Transforms [i] ["smsy"]!=0||Transforms [i] ["smsz"]!=0)
							sinScale.Set(
								((Transforms [i] ["ssMult"]*jointNumber)+Transforms [i] ["smsx"]) * Mathf.Sin (((Transforms [i] ["sssx"] * timer + Transforms [i] ["sosx"] + (Transforms [i] ["srosx"]*jointOffset)) + (Transforms [i] ["sfsx"] * jointNumber))),
								((Transforms [i] ["ssMult"]*jointNumber)+Transforms [i] ["smsy"]) * Mathf.Sin (((Transforms [i] ["sssy"] * timer + Transforms [i] ["sosy"] + (Transforms [i] ["srosy"]*jointOffset)) + (Transforms [i] ["sfsy"] * jointNumber))),
								((Transforms [i] ["ssMult"]*jointNumber)+Transforms [i] ["smsz"]) * Mathf.Sin (((Transforms [i] ["sssz"] * timer + Transforms [i] ["sosz"] + (Transforms [i] ["srosz"]*jointOffset)) + (Transforms [i] ["sfsz"] * jointNumber)))
							);

						if (Transforms [i] ["sus"] != 0) {
							sinScale.Set (sinScale.x, sinScale.x, sinScale.x);
						}

						if (Transforms [i] ["length"] != 0) {
							if (g.GetComponent<Joint> ().childJoint != null) {
								float off;
								off = ((Transforms [i] ["lMult"] * jointNumber) + Transforms [i] ["sml"]) * Mathf.Sin (((Transforms [i] ["ssl"] * timer + Transforms [i] ["sol"] + (Transforms [i] ["srol"] * jointOffset) + (Transforms [i] ["saol"]*jointOffset2)) + (Transforms [i] ["sfl"] * jointNumber)));
								g.GetComponent<Joint> ().childJoint.transform.localPosition = new Vector3 (0, Transforms [i] ["length"] + off, 0);
								Vector3 sc = g.GetComponent<Joint> ().scalar.transform.localScale;
								g.GetComponent<Joint> ().scalar.transform.localScale = new Vector3 (sc.x, Transforms [i] ["length"] + off, sc.z);
							}
						}


						rotateOffset.Set (Transforms [i] ["orx"] * timer, Transforms [i] ["ory"] * timer, Transforms [i] ["orz"] * timer);
						g.transform.localEulerAngles = rotate+init+sinRotate+noiseRotate+rotateOffset;

		//				g.transform.Rotate (rotateOffset.x,rotateOffset.y,rotateOffset.z);
						scale.Set(Transforms [i] ["scale"] + Transforms [i] ["sx"],Transforms [i] ["scale"] + Transforms [i] ["sy"],Transforms [i] ["scale"] + Transforms [i] ["sz"]);
						Vector3 overallScale = sinScale + scale;
					
						if (Transforms [i] ["cr"] != 0 || Transforms [i] ["cg"] != 0 || Transforms [i] ["cb"] != 0){

							Color initCol = new Color (
									Transforms [i] ["cr"], 
									Transforms [i] ["cg"], 
									Transforms [i] ["cb"]);
							float hue, S, V;
							Color.RGBToHSV (initCol, out hue, out S, out V);
							float off = ((Transforms [i] ["cMult"] * jointNumber) + Transforms [i] ["smc"]) * Mathf.Sin (((Transforms [i] ["ssc"] * timer + Transforms [i] ["soc"] + (Transforms [i] ["sroc"] * jointOffset) + (Transforms [i] ["saoc"]*jointOffset2)) + (Transforms [i] ["sfc"] * jointNumber)));
							float fOff = off-Mathf.Floor(off);
							if(fOff<0)
								fOff -= Mathf.Floor(off);
							for (int k = 0; k < g.GetComponent<Joint> ().scalar.transform.childCount; k++) {
								if(g.GetComponent<Joint> ().scalar.transform.GetChild(k).GetComponent<MeshRenderer> ()!=null)
									g.GetComponent<Joint> ().scalar.transform.GetChild(k).GetComponent<MeshRenderer> ().material.color = Color.HSVToRGB (fOff, 1, 1);;
							}
							for (int k = 0; k < g.GetComponent<Joint> ().rotator.transform.childCount; k++) {
								if(g.GetComponent<Joint> ().rotator.transform.GetChild(k).GetComponent<MeshRenderer> ()!=null)
									g.GetComponent<Joint> ().rotator.transform.GetChild(k).GetComponent<MeshRenderer> ().material.color = Color.HSVToRGB (fOff, 1,1);;
							}
//							g.GetComponent<Joint> ().scalar.transform.GetChild (0).GetComponent<MeshRenderer> ().material.color = Color.HSVToRGB (fOff, 1, 1);
						}

						if(!overallScale.Equals(Vector3.one))
							g.transform.localScale = overallScale;// Vector3.Scale(overallScale , new Vector3 (Transforms [i] ["sx"], Transforms [i] ["sy"], Transforms [i] ["sz"]));
						
					}
				}
			}

		}
		
	}
}
