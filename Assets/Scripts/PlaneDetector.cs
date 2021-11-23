using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;

public class PlaneDetector : MonoBehaviour
{
    private bool run = true;

    public GameObject DetectedPlanePrefab;
    public List<GameObject> planes = new List<GameObject>();
    private List<DetectedPlane> m_NewPlanes = new List<DetectedPlane>();

    void Start()
    {
        
    }

    void Update()
    {
        if (Session.Status != SessionStatus.Tracking)
        {
            return;
        }

        if (run)
        {
            Session.GetTrackables<DetectedPlane>(m_NewPlanes, TrackableQueryFilter.New);
            for (int i = 0; i < m_NewPlanes.Count; i++)
            {
                GameObject planeObject =
                    Instantiate(DetectedPlanePrefab, Vector3.zero, Quaternion.identity, transform);
                planeObject.GetComponent<GoogleARCore.Examples.Common.DetectedPlaneVisualizer>().Initialize(m_NewPlanes[i]);
                planes.Add(planeObject);
            }
        }
    }

    public void stop()
    {
        run = false;
        foreach (GameObject go in planes)
        {
            Destroy(go);
        }
    }
}
