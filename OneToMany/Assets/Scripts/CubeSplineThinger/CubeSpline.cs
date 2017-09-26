using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpline : MonoBehaviour {

    public Curve curve;
    public int maxCubes;
	[Header("Cubes")]
    public GameObject prefab;
    public AnimationCurve sizeDistribution;
    public float minSize;
    public float maxSize;
	public AnimationCurve speedBySize;
	public float minDist;
	public float maxDist;
    private int currentCubes;


    void Start(){
        for (int i = 0; i < maxCubes; i++){
            SpawnCubeNear(curve, (1/(float)maxCubes) *i);
        }
    }

	void Update(){
		if(currentCubes < maxCubes){
            SpawnCubeNear(curve, 0);
        }
	}

	public void CubeRemoved(GameObject cube){
        currentCubes--;
    }

	private void SpawnCubeNear(Curve curve, float position){
        Vector3 p = curve.Get(position);
        var go = GameObject.Instantiate(prefab, p, Quaternion.LookRotation(Random.onUnitSphere,Random.onUnitSphere));
		go.transform.parent = transform;
		go.transform.position = p;
        var rand = Random.value;
        go.transform.localScale = Vector3.one * (minSize + (maxSize-minSize)*sizeDistribution.Evaluate(rand));
        var fc= go.GetComponent<FollowCurveSteering>();
        fc.minDist = minDist;
		fc.maxDist = maxDist*(1+2*(1-rand));
        fc.speed = speedBySize.Evaluate(rand);
        fc.curveToFollow = curve;
        fc.currentU = position;
        var lr = go.AddComponent<LinearRotatation>();
        lr.rate = Random.Range(0, 30.0f+30*(1-rand));
        currentCubes++;
    }
}
