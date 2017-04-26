using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float m_DampTime = 0.2f;                 
    public float m_ScreenEdgeBuffer = 4f;          
    public float m_MinSize = 6.5f;
    public float m_defualtCameraSize = 13f;
    public Transform[] m_Targets; 


    private Camera m_Camera;                        
    private float m_ZoomSpeed;                      
    private Vector3 m_MoveVelocity;               
    private Vector3 m_DesiredPosition;            

    public bool coopMode = false;
    public Transform northWall;
    public Transform southWall;
    public Transform westWall;
    public Transform easthWall;

    private void Awake()
    {
        m_Camera = GetComponentInChildren<Camera>();
    }


    private void FixedUpdate()
    {
        Move();
        Zoom();
    }


    private void Move()
    {
        FindAveragePosition();

        transform.position = Vector3.SmoothDamp(transform.position, m_DesiredPosition, ref m_MoveVelocity, m_DampTime);
    }


    private void FindAveragePosition()
    {
        Vector3 averagePos = new Vector3();
        int numTargets = 0;

        for (int i = 0; i < m_Targets.Length; i++)
        {

            if (!m_Targets[i].gameObject.activeSelf)
                continue;


            averagePos += m_Targets[i].position;
            numTargets++;
        }


        if (numTargets > 0)
            averagePos /= numTargets;


        averagePos.y = transform.position.y;


        m_DesiredPosition = averagePos;
    }


    private void Zoom()
    {
        if (coopMode)
        {
            float requiredSize = FindRequiredSize();
            m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);

        }
        else
        {

            float requiredSize = FindRequiredSizeForPlayers();
            m_Camera.orthographicSize = Mathf.SmoothDamp(m_Camera.orthographicSize, requiredSize, ref m_ZoomSpeed, m_DampTime);
        }

    }
    float[] distanceWalls = new float[5];

    private float FindRequiredSizeForPlayers()
    {
        float size = 0f;

        distanceWalls[0] = m_defualtCameraSize;

        distanceWalls[1] = Mathf.Abs(northWall.position.z - m_Targets[0].transform.position.z);

        distanceWalls[2] = Mathf.Abs(westWall.position.x - m_Targets[0].transform.position.x);

        distanceWalls[3] = Mathf.Abs(southWall.position.z - m_Targets[0].transform.position.z);
 
        distanceWalls[4] = Mathf.Abs(easthWall.position.x - m_Targets[0].transform.position.x);
   

        size = Mathf.Min(distanceWalls);


        return size;


    }

    private float FindRequiredSize()
    {

        Vector3 desiredLocalPos = transform.InverseTransformPoint(m_DesiredPosition);


        float size = 0f;


        for (int i = 0; i < m_Targets.Length; i++)
        {

            if (!m_Targets[i].gameObject.activeSelf)
                continue;


            Vector3 targetLocalPos = transform.InverseTransformPoint(m_Targets[i].position);


            Vector3 desiredPosToTarget = targetLocalPos - desiredLocalPos;


            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.y));


            size = Mathf.Max(size, Mathf.Abs(desiredPosToTarget.x) / m_Camera.aspect);
        }


        size += m_ScreenEdgeBuffer;


        size = Mathf.Max(size, m_MinSize);

        return size;
    }


    public void SetStartPositionAndSize()
    {
        FindAveragePosition();

        transform.position = m_DesiredPosition;

        m_Camera.orthographicSize = FindRequiredSize();
    }
}