using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GoogleARCore;


public class ControllerFace : MonoBehaviour
{
    private List<AugmentedFace> detectedFaces = new List<AugmentedFace>();
    public GameObject faceGO;
    public GameObject nose;
    public GameObject leftEar;
    public GameObject rightEar;
    public GameObject point;

    private List<Vector3> m_MeshVertices = new List<Vector3>();
    private List<Vector3> m_MeshNormals = new List<Vector3>();
    private List<Vector2> m_MeshUVs = new List<Vector2>();
    private List<int> m_MeshIndices = new List<int>();
    private Mesh m_Mesh = null;
    private bool m_MeshInitialized = false;


    // Start is called before the first frame update
    void Start()
    {
        m_Mesh = new Mesh();
        faceGO.GetComponent<MeshFilter>().mesh = m_Mesh;

    }

    // Update is called once per frame
    void Update()
    {

        Session.GetTrackables<AugmentedFace>(
                detectedFaces, TrackableQueryFilter.All);

        foreach (var face in detectedFaces)
        {
            if (face.TrackingState == TrackingState.Tracking)
            {
                faceGO.transform.position = face.CenterPose.position;
                faceGO.transform.rotation = face.CenterPose.rotation;

                Pose regionPose = face.GetRegionPose(AugmentedFaceRegion.NoseTip);
                nose.transform.position = regionPose.position;
                nose.transform.rotation = regionPose.rotation;

                regionPose = face.GetRegionPose(AugmentedFaceRegion.ForeheadLeft);
                leftEar.transform.position = regionPose.position;
                leftEar.transform.rotation = regionPose.rotation;

                regionPose = face.GetRegionPose(AugmentedFaceRegion.ForeheadRight);
                rightEar.transform.position = regionPose.position;
                rightEar.transform.rotation = regionPose.rotation;

                face.GetVertices(m_MeshVertices);
                face.GetNormals(m_MeshNormals);

                if (!m_MeshInitialized)
                {
                    face.GetTextureCoordinates(m_MeshUVs);
                    face.GetTriangleIndices(m_MeshIndices);

                    // Only update mesh indices and uvs once as they don't change every frame.
                    m_MeshInitialized = true;
                }

                m_Mesh.Clear();
                m_Mesh.SetVertices(m_MeshVertices);
                m_Mesh.SetNormals(m_MeshNormals);
                m_Mesh.SetTriangles(m_MeshIndices, 0);
                m_Mesh.SetUVs(0, m_MeshUVs);

                m_Mesh.RecalculateBounds();

            }
        }
    }
}
