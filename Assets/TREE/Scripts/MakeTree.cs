using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace TREESharp{
	
public class MakeTree : MonoBehaviour {
	
	TREE tree;
	public GameObject defaultJoint;
	public TreeTransform xForm { get; set; }

	public string joints= "10",rads= "1",angles= "0",length= "1",divs= "1",start = "0",end = "-1";

	public string[] selectJoints;
	public string[] transformJoints;

	public bool animate = true;

	public float counter { get; set; }
	public float countSpeed { get; set; }

	public float timeScale = 1;

	public bool reSetupXform = true;
	public bool rebuildTree = true;

	void Start () {

	}

	public void buildTree(){
		
		if (this.GetComponent<Joint> () != null) {
			if (this.GetComponent<Joint> ().limbs.Count > 0) {
				this.GetComponent<Joint> ().limbs.Clear ();
			}
		}

		if (this.gameObject.GetComponent<TREE> () == null) {
			tree = this.gameObject.AddComponent<TREE> ();
		}
		else {
			for (int i = 0; i < this.transform.childCount; i++) {
				Destroy (this.transform.GetChild (0).gameObject);
			}
			tree = GetComponent<TREE> ();
		}
			
		tree.setDefaultJoint(defaultJoint);

		tree.generate (
			"joints",	joints,
			"rads",		rads,
			"angles",	angles,
			"length",	length,
			"divs",		divs,
			"start",	start,
			"end",		end
		);

		tree.jointDictionary.Clear ();
		TREEUtils.makeDictionary (tree.gameObject);

	}
		
	void Update () {
		if (rebuildTree) {
			rebuildTree = false;
			buildTree ();
			reSetupXform = true;
		} else if (reSetupXform) {
			reSetupXform = false;
			if (this.gameObject.GetComponent<TreeTransform> () == null) {
				xForm = this.gameObject.AddComponent<TreeTransform> ();
			} else
				xForm = this.gameObject.GetComponent<TreeTransform> ();
			
			xForm.Setup (selectJoints, transformJoints, tree);
			Animate ();
		} else if (animate && !reSetupXform && !rebuildTree)
			xForm.Animate (Time.time * timeScale);
//		else if (!reSetupXform && !rebuildTree) {
//			Animate ();
//			countSpeed = 0;
//			counter = 0;
//		}
//		
	}

	public void Animate(){
		counter += countSpeed * timeScale;
		xForm.Animate (counter);
	}

	public void Animate(float offset){
		counter += offset;
		xForm.Animate (counter);
	}
		
}
}