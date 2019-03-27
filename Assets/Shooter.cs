using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour {


    GameObject prefab;
    float force = 150f;
    float speed = 30f;
    private Camera cam;
    private GameObject moon;
    private Vector3 target;
    string r = "23";
    string g = "26";
    string b = "135";

	// Use this for initialization
	void Start () {
        prefab = Resources.Load("projectile") as GameObject;
        moon = GameObject.FindGameObjectWithTag("moon");
        cam = Camera.main;
        target = moon.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        
        ShootStar(r, g, b);
	}


    void ShootStar(string r, string g, string b)
    {
    
        if (Input.anyKeyDown)
        {
            GameObject projectile = Instantiate(prefab) as GameObject;
            Quaternion towardsMoon = Quaternion.LookRotation(target, Vector3.up);
            projectile.transform.position = this.transform.position + new Vector3(Random.Range(-10.0f, 10.0f), 0, 0);
            projectile.transform.rotation = towardsMoon;
            projectile.transform.LookAt(moon.transform);
            float s = Random.Range(0.5f, 5.0f);
            projectile.transform.localScale = new Vector3(s, s, s);
            Rigidbody rb = projectile.GetComponent<Rigidbody>();
            rb.velocity = projectile.transform.forward * speed;
            rb.mass = Random.Range(0.8f, 1.2f);

            float colorR = float.Parse(r);
            float colorG = float.Parse(g);
            float colorB = float.Parse(b);

            Renderer rend = projectile.GetComponent<Renderer>();
            rend.material.SetColor("_Color", new Color(colorR / 255f, colorG / 255f, colorB / 255f, 1f));

        }
    
    }
}
