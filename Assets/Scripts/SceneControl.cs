using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneControl : MonoBehaviour
{
	public ImageSynthesis synth;
	public GameObject comboxp1;
	public GameObject comboxp2;
	public GameObject container;
	public int trainingImages;
    public int valImages;
	static int maxCombox = 13;
	private GameObject[] comboxes_p1;
	private GameObject[] comboxes_p2;
	public Vector3 com_p1_pos = new Vector3(-2218f, 686.28f, -2653f);
	public Vector3 com_p1_rot = new Vector3(0f, 0f, 90f);
	public float com_p1_scale = 0.9f;
	public Vector3 com_p2_pos = new Vector3(-2218f, -102.48f, -2656f);
	public Vector3 com_p2_rot = new Vector3(0f, 0f, 90f);
	public float com_p2_scale = 0.9f;
	public int z_offset;
	public int x_offset = 200; //l:-2483, r:-1953
	//public Material mat;
	
	private int frameCount = 0;
	public bool save = false;
    // Start is called before the first frame update
    void Start()
    {
		GenerateAllComboxes();
    }

    // Update is called once per frame
    void Update()
    {
		if (frameCount < trainingImages + valImages) {
            if (frameCount % 1 == 0) {
				RandomAwake(comboxes_p1, comboxes_p2);
				RandomMovePosition(comboxes_p1, comboxes_p2, x_offset);
				synth.OnSceneChange();
                Debug.Log($"FrameCount: {frameCount}");
            }
            frameCount++;
            if (save) {
                if (frameCount < trainingImages) {
                    string filename = $"image_{(frameCount+0).ToString().PadLeft(5, '0')}";
                    synth.Save(filename, 1280, 720, "captures1/train", 2);
                }
                else if (frameCount < trainingImages + valImages) {
                    int valFrameCount = frameCount - trainingImages;
                    string filename = $"image_{(valFrameCount+0).ToString().PadLeft(5, '0')}";
                    synth.Save(filename, 1280, 720, "captures1/val", 2);
                }

            }
        }

    }
	
	void GenerateAllComboxes(){
		// Put container in the scene
		/*Vector3 pos = new Vector3(0.0f,-200f,0.0f);
		var rot = Quaternion.Euler(new Vector3(90.0f,0.0f,0.0f));	
		var newContainer = Instantiate(container, pos, rot);
		newContainer.GetComponent<Renderer>().material.color = new Color(0.1f, 0.1f, 0.1f);
		*/
		//Generate comboxes
		comboxes_p1 = generatePart(comboxp1, com_p1_pos, com_p1_rot, com_p1_scale, 8, 0.8f);
		comboxes_p2 = generatePart(comboxp2, com_p2_pos, com_p2_rot, com_p2_scale, 9, 0.1f);
						
	}
	
	GameObject[] generatePart(GameObject obj, Vector3 pos, Vector3 rot, float scale, int layer, float color){
		GameObject[] comboxes = new GameObject[maxCombox];
		for(int i = 0; i < maxCombox; i++) {			
			// Position
			float newX, newY, newZ;
			newX = pos.x; //+ Random.Range(-x_offset, x_offset);
			newY = pos.y;
			newZ = pos.z - z_offset * i;
			Vector3 newPos = new Vector3(newX, newY, newZ);

			// Rotation
			var newRot = Quaternion.Euler(rot);

			var newCombox = Instantiate(obj, newPos, newRot);
			
			// Color
			var newColor = new Color(color, color, color);
			//GetComponent<Renderer>().material.color = newColor;
			newCombox.GetComponentInChildren<Renderer>().material.color = newColor;
			
			comboxes[i] = newCombox;
			//comboxes[i].SetActive(false);
			comboxes[i].layer = layer;
		}
		return comboxes;
	}
	
	void RandomAwake(GameObject[] objects){
		for (int i=0; i<objects.Length; i++){
			objects[i].SetActive(false);
		}
		int num_visible = Random.Range(1, objects.Length);
		for (int i=0; i<num_visible; i++){
			objects[Random.Range(0, objects.Length)].SetActive(true);
		}
	}
	
	void RandomAwake(GameObject[] objects1, GameObject[] objects2){
		for (int i=0; i<objects1.Length; i++){
			objects1[i].SetActive(false);
			objects2[i].SetActive(false);
		}
		int num_visible = Random.Range(1, objects1.Length);
		for (int i=0; i<num_visible; i++){
			int idx = Random.Range(0, objects1.Length);
			objects1[idx].SetActive(true);
			objects2[idx].SetActive(true);
		}
	}
	
	void RandomMovePosition(GameObject[] objects, float x_offset){
		 foreach (GameObject obj in objects)
		 {
			 // Checks if the current combox is active
			 if (obj.activeInHierarchy)
			 {	
				var pos = obj.transform.position;
				pos.x = pos.x + Random.Range(-x_offset, x_offset);
				obj.transform.position = pos;
			 }
		 }
	}	
	void RandomMovePosition(GameObject[] objects1, GameObject[] objects2, float x_offset){
		 for (int i=0; i<objects1.Length; i++)
		 {
			 // Checks if the current combox is active
			 if (objects1[i].activeInHierarchy)
			 {	
				var pos1 = objects1[i].transform.position;
				var pos2 = objects2[i].transform.position;
				float tmp = Random.Range(-x_offset, x_offset);
				pos1.x = com_p1_pos.x + tmp;
				pos2.x = com_p2_pos.x + tmp;
				objects1[i].transform.position = pos1;
				objects2[i].transform.position = pos2;
			 }
		 }
	}
}	
