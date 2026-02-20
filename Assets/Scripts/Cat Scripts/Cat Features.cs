using UnityEngine;

public class CatFeatures : MonoBehaviour
{
    public float personality = 0.5f;
    public float ovenTime = 0.0f;

    ChatShaderCtrl CSC = null;
    bool once = true;
    float scaler = 0.285f;

    [SerializeField] private Vector3 doughScale;

    void Start()
    {
        CSC = GetComponent<ChatShaderCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (ovenTime >= 5 && once)
        {
            CSC.shellMesh = Resources.Load<Mesh>("Meshes/Cube.001");
            CSC.furDensity = 100;
            GetComponent<MeshFilter>().mesh = Resources.Load<Mesh>("Meshes/Cube.001");

            SphereCollider sphere = GetComponent<SphereCollider>();
            if (sphere)
            {
                Destroy(sphere);
            }

            BoxCollider box = GetComponent<BoxCollider>();
            if (!box)
            {
                box = gameObject.AddComponent<BoxCollider>();
            }

            box.center = new Vector3(9.44926171e-07f, 0.0161516126f, -0.65160954f);
            box.size = new Vector3(1.68960726f, 8.04663944f, 7.30896902f);

            transform.localScale = new Vector3(doughScale.x*scaler, doughScale.y*scaler, doughScale.z*scaler);
            transform.localRotation = Quaternion.Euler(90f, 0f, 0f);
            once = false;


        }
        else if(ovenTime < 5)
        {
            doughScale = transform.localScale;
        }
    }
}
