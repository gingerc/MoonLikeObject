#if !BESTHTTP_DISABLE_SOCKETIO

using System.Collections;
using System;
using System.Collections.Generic;

using UnityEngine;
using BestHTTP.SocketIO;


public class UnitySocketClient : MonoBehaviour {

    GameObject prefab;
    //float force = 150f;
    float speed = 30f;
    //private Camera cam;
    private GameObject moon;
    private Vector3 target;


    #region Fields

    /// <summary>
    /// The Socket.IO manager instance.
    /// </summary>
    private SocketManager Manager;

    //received message
 //   private string message = string.Empty;
    private string notification = "The Moon received your message about ";

    #endregion

    #region Unity Events

    // Use this for initialization
    void Start () {
		
        // Change an option to show how it should be done
        SocketOptions options = new SocketOptions();
        options.AutoConnect = false;
        options.ConnectWith = BestHTTP.SocketIO.Transports.TransportTypes.WebSocket;

        // Create the Socket.IO manager
        Manager = new SocketManager(new Uri("http://10.25.138.1:8000/socket.io/"), options);

        //if reveived "new message" then call the OnMessageReceived function
        Manager.Socket.On("new message", OnMessageReceived);

        // The argument will be an Error object.
        Manager.Socket.On(SocketIOEventTypes.Error, (socket, packet, args) => Debug.LogError(string.Format("Error: {0}", args[0].ToString())));
        // We set SocketOptions' AutoConnect to false, so we have to call it manually.
        Manager.Open();



        //shooter related
        prefab = Resources.Load("projectile") as GameObject;
        moon = GameObject.FindGameObjectWithTag("moon");
        //cam = Camera.main;
        target = moon.transform.position;
	}


	
	// Update is called once per frame
	void Update () {
        // Leaving this sample, close the socket
        if (Input.GetKeyDown(KeyCode.Escape)){
            Manager.Close();
        }
         
		
	}

    #endregion

    #region Send Back to Server




    void NotifyWebClient(Dictionary<string, object> data)
    {
        //b1. emit a message back to server saying the message is received
        var username = data["name"] as string;
        var msg = data["msg"] as string;

        Manager.Socket.Emit("notif", username+ ","+ notification + " - " + msg + " - ");
       
    }

    #endregion

    #region Custom SocketIO Events

    void OnMessageReceived(Socket socket, Packet packet, params object[] args)
    {
        var data = args[0] as Dictionary<string, object>;

        var username = data["name"] as string;
        var msg = data["msg"] as string;
        var r = data["r"] as string;
        var g = data["g"] as string;
        var b = data["b"] as string;

            
        //print the received message to console
       // Debug.Log("received a message!");
        Debug.Log(username + ":" + msg);

        ShootStar(r,g,b);


        NotifyWebClient(data);
    }

    #endregion


    void ShootStar(string r, string g, string b){
        
        GameObject projectile = Instantiate(prefab) as GameObject;
        Quaternion towardsMoon = Quaternion.LookRotation(target, Vector3.up);
        projectile.transform.position = this.transform.position + new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 0, 0);
        projectile.transform.rotation = towardsMoon;
        projectile.transform.LookAt(moon.transform);
        float s = UnityEngine.Random.Range(0.5f, 5.0f);
        projectile.transform.localScale = new Vector3(s, s, s);
        Rigidbody rb = projectile.GetComponent<Rigidbody>();
        rb.velocity = projectile.transform.forward * speed;
        rb.mass = UnityEngine.Random.Range(0.8f, 1.2f);

        float colorR = float.Parse(r);
        float colorG = float.Parse(g);
        float colorB = float.Parse(b);

        Renderer rend = projectile.GetComponent<Renderer>();
        rend.material.SetColor("_Color", new Color(colorR/ 255f, colorG/ 255f, colorB/ 255f, 1f));
    }

}

#endif