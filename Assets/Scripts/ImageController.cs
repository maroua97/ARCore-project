using GoogleARCore;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ImageController : MonoBehaviour
    
{
    private List<AugmentedImage> images = new List<AugmentedImage>();
    public List<GameObject> gameObjects = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Session.GetTrackables<AugmentedImage>(images, TrackableQueryFilter.Updated);
        if (images.Count >0 )
        {
            for (int i = 0; i < images.Count; i++)
            {
                int index = images[i].DatabaseIndex;
                gameObjects[index].transform.position = images[i].CenterPose.position;
                gameObjects[index].transform.rotation = images[i].CenterPose.rotation;
                gameObjects[index].SetActive(true);
            } 
            //images[i].DatabaseIndex, images[i].CenterPose.position (or.rotation)
        }
    }
}
