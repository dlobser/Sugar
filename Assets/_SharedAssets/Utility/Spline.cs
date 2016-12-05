using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class Spline : ScriptableObject {

	public List<Transform> _controlPointsList = new List<Transform>();
    public List<Vector3> controlPointsList = new List<Vector3>();
    public int detail = 100;
    public float micro = 1f;
//	public static Vector3[] points;
	
	public void init(List<Transform> cp, int _detail){
		detail = _detail;
		_controlPointsList = cp;
        NewCP(micro);
	}
    public void init(List<Transform> cp, int _detail, float _micro)
    {
        detail = _detail;
        _controlPointsList = cp;
//        if(_micro >0)
//        micro = _micro;
//		Debug.Log (micro);
        NewCP(micro);
    }

    void NewCP(float micro)
    {
        List<int> ind = new List<int>();
        ind.Add(0);
        for (int i = 0; i < _controlPointsList.Count; i++)
        {
            controlPointsList.Add(_controlPointsList[i].transform.position);
            if (i + 1 < _controlPointsList.Count)
            {
                if (micro > 0.3f * Vector3.Distance(_controlPointsList[i].position, _controlPointsList[i + 1].position))
                {
                    
                }
                else
                {
                    int count = Mathf.FloorToInt((Vector3.Distance(_controlPointsList[i].position, _controlPointsList[i+1].position)) / micro);
                    float _gap = (Vector3.Distance(_controlPointsList[i].position, _controlPointsList[i+1].position)-2f*micro) / (count-2);
//                    Debug.Log(_gap);
                    for (int j = 0; j < count-1; j++)
                    {
                        Vector3 tmp = Vector3.Lerp(_controlPointsList[i].position, _controlPointsList[i+1].position, (micro+j * _gap) / (Vector3.Distance(_controlPointsList[i].position, _controlPointsList[i+1].position)));
                        controlPointsList.Add(tmp);
                    }
                    ind.Add(ind[i] + count);
                }
            }
            else
            {
                if (micro > 0.3f * Vector3.Distance(_controlPointsList[i].position, _controlPointsList[0].position))
                {
                   
                }
                else
                {
                    int count = Mathf.FloorToInt((Vector3.Distance(_controlPointsList[i].position, _controlPointsList[0].position)) / micro);
                    float _gap = (Vector3.Distance(_controlPointsList[i].position, _controlPointsList[0].position) - 2f * micro) / (count-2);
                    for (int j = 1; j < count-1; j++)
                    {
                        Vector3 tmp = Vector3.Lerp(_controlPointsList[i].position, _controlPointsList[0].position, ((micro + j * _gap) / (Vector3.Distance(_controlPointsList[i].position, _controlPointsList[0].position))));
                        controlPointsList.Add(tmp);
                    }
                }
            }
        }
        for (int i =0; i < ind.Count; i++)
        {
            if (i == 0)
            {
                controlPointsList[ind[i]] = Vector3.Lerp(controlPointsList[ind[i]],Vector3.Lerp(controlPointsList[ind[i] + 1], controlPointsList[controlPointsList.Count - 1],0.5f),0.5f);
            }
            else
            {
               // Debug.Log(controlPointsList[ind[i]]);
                controlPointsList[ind[i]] = Vector3.Lerp(controlPointsList[ind[i]], Vector3.Lerp(controlPointsList[ind[i] + 1], controlPointsList[ind[i] - 1], 0.5f), 0.5f);
               // Debug.Log(controlPointsList[ind[i]]);
            }
        }
    }
	public void setDetail(int d){
		detail = d;
	}


	public Vector3 ReturnCatmullRom(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3) {
		Vector3 a = 0.5f * (2f * p1);
		Vector3 b = 0.5f * (p2 - p0);
		Vector3 c = 0.5f * (2f * p0 - 5f * p1 + 4f * p2 - p3);
		Vector3 d = 0.5f * (-p0 + 3f * p1 - 3f * p2 + p3);
		
		Vector3 pos = a + (b * t) + (c * t * t) + (d * t * t * t);
		
		return pos;
	}
	
	public Vector3 getPoint ( float t ) {
		
		float point = ( controlPointsList.Count - 1 ) * t;
		int intPoint = (int)Mathf.Floor( point );
		float weight = point - intPoint;
		
		Vector3 point0 = controlPointsList[ intPoint == 0 ? intPoint : intPoint - 1 ];
		Vector3 point1 = controlPointsList[ intPoint ];
		Vector3 point2 = controlPointsList[ intPoint > controlPointsList.Count - 2 ? controlPointsList.Count - 1 : intPoint + 1 ];
		Vector3 point3 = controlPointsList[ intPoint > controlPointsList.Count - 3 ? controlPointsList.Count - 1 : intPoint + 2 ];
		
		return ReturnCatmullRom( weight, point0, point1, point2, point3 );
	}

	public Vector3 getPointClosed  ( float t ) {
		
		float point = ( controlPointsList.Count - 1 ) * t;

		int intPoint = (int)Mathf.Floor( point );
		float weight = point - intPoint;

		intPoint += (int)intPoint > 0 ? 0 : (int)( Mathf.Floor( Mathf.Abs( intPoint ) / (int)controlPointsList.Count ) + 1 ) * (int)controlPointsList.Count;
		
		Vector3 point0 = controlPointsList[ ( intPoint - 1 ) % controlPointsList.Count ];
		Vector3 point1 = controlPointsList[ ( intPoint     ) % controlPointsList.Count ];
		Vector3 point2 = controlPointsList[ ( intPoint + 1 ) % controlPointsList.Count ];
		Vector3 point3 = controlPointsList[ ( intPoint + 2 ) % controlPointsList.Count ];

		Vector3 vector = ReturnCatmullRom( weight, point0, point1, point2, point3);
		
		return vector;
		
	}

	public Vector3 getLinearPoint(float t){

		float point = ( controlPointsList.Count - 1 ) * t;
		
		int intPoint = (int)Mathf.Floor( point );
		float weight = point - intPoint;
		
		intPoint += (int)intPoint > 0 ? 0 : (int)( Mathf.Floor( Mathf.Abs( intPoint ) / (int)controlPointsList.Count ) + 1 ) * (int)controlPointsList.Count;
		
		Vector3 point0 = controlPointsList[ ( intPoint - 1 ) % controlPointsList.Count ];
		Vector3 point1 = controlPointsList[ ( intPoint     ) % controlPointsList.Count ];
		Vector3 point2 = controlPointsList[ ( intPoint + 1 ) % controlPointsList.Count ];
		Vector3 point3 = controlPointsList[ ( intPoint + 2 ) % controlPointsList.Count ];
		
		Vector3 vector = Vector3.Lerp (point1, point2, weight);
		
		return vector;

	}


	public Vector3[] getPoints(){
		Vector3[] points = new Vector3[detail];
		for (int i = 0; i < detail; i++) {
			points[i] = getPoint ((float)i/(detail-1));
		}
		return points;
	}

	public float getLength(){
		float len = 0;
		if (controlPointsList.Count > 1) {
			for (int i = 1; i <   controlPointsList.Count; i++) {
				Vector3 thisPos = controlPointsList [i];
				Vector3 prevPos = controlPointsList [i - 1];
				len += Vector3.Distance (thisPos, prevPos);
			}
		}
		return len;
	}


	void OnDrawGizmos() {

		Gizmos.color = Color.red;
		
		//Draw a sphere at each control point
		for (int i = 0; i < controlPointsList.Count; i++) {
			Gizmos.DrawWireSphere(controlPointsList[i], 0.3f);
		}

		for (int i = 1; i < detail; i++) {
			float j = i/(float)detail;
			float k = (i-1)/(float)detail;
			Vector3 J = getPoint (j);
			Vector3 K = getPoint (k);
			Gizmos.DrawLine(J,K);
		}
	}
}
