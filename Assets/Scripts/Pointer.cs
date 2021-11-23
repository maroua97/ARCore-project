using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;
using UnityEngine.UI;
using System;

public class Pointer : MonoBehaviour
{
    public GameObject pointer;
    public GameObject system;
    public Camera MainCamera;
    private bool set = false;
    public Text info;

    public List<Info> informations = new List<Info>
    {
    new Info("Sun", "Sun \n The star at the center of the solar system \n Radius ~ 695 000 km"),
    new Info("Mercury", "Mercury \n The smallest and closest planet to the sun \n Radius ~ 2440 km \n Satellites: None "),
    new Info("Venus","Venus \n The second planet from the Sun. \n It is named after the Roman goddess of love and beauty.\n Radius ~ 6052 km \n Satellites: None "),
    };
    //public List<AudioSource> audioSources = new List<AudioSource>();

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (!set)
        {
            //if (Session.State != SessionStates.Tracking) { return;}
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds;
            if (Frame.Raycast(Screen.width / 2, Screen.height / 2, raycastFilter, out hit))
            {
                Vector3 pt = hit.Pose.position;
                pointer.transform.position = Vector3.Lerp(pointer.transform.position, pt, Time.smoothDeltaTime * 20);

            }
            placeObject();
        }
        else
        {
            playInfo();
        }
    }

    void placeObject()
    {
        if (Input.touchCount >= 1 && Input.GetTouch(0).phase == TouchPhase.Began)
        {

            Touch touch = Input.GetTouch(0);
            TrackableHit hit;
            TrackableHitFlags raycastFilter = TrackableHitFlags.PlaneWithinBounds | TrackableHitFlags.PlaneWithinPolygon;
            if (Frame.Raycast(touch.position.x, touch.position.y, raycastFilter, out hit))
            {
                DetectedPlane plane = (DetectedPlane)hit.Trackable;
                system.transform.position = plane.CenterPose.position;
                set = true;
                system.transform.Translate(0.0f, 1f, 0.0f);
                system.SetActive(true);
                pointer.SetActive(false);
                GetComponent<PlaneDetector>().stop();
            }

        }
    }

    void playInfo()
    {
        int children = system.transform.childCount;
        for (int i = 0; i < children; ++i)
        {
            GameObject child = system.transform.GetChild(i).gameObject;
            float distance = Vector3.Distance(MainCamera.transform.position, child.transform.position) - child.transform.localScale.x;

            GameObject text = child.transform.GetChild(0).gameObject;


            if (distance < 0.1f)
            {
                //audioSource.Play();
                text.transform.LookAt(MainCamera.transform);
                text.transform.Rotate(0.0f, 180.0f, 0.0f, Space.Self);
                text.SetActive(true);
                Info information = informations.Find(info => info.gameObject == child.name);              
                info.text = information.text;
                
            }
            else
            {
                text.SetActive(false);
                info.text = "Welcome to our Solar System";
            }
        }
    }
}

public class Info
{
    public string gameObject;
    public string text;

    public Info(string go, string txt)
    {
        this.gameObject = go;
        this.text = txt;
    }
}

