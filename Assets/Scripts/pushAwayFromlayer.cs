using UnityEngine;

public class PushAwayFromLayer : MonoBehaviour
{
	[SerializeField] private float force = 6000.0f;
	[SerializeField] private float radius = 100.0f;
    [SerializeField] private float liftingForce = 0.5f;
    [SerializeField] private float overlapSphereRadius = 20.0f;
    [SerializeField] private float thrust = 2000;

    private CapsuleCollider skillMaker;
    private int turningBlocks;//obracanie blkow dla wygladu wip

    private void Start ()
    {
        skillMaker = GetComponent<CapsuleCollider>();
        //turningBlocks = Random.Range(0, 10);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            skillMaker.radius = 100;
        }
        else
        {
            skillMaker.radius = 0;
        }

        if(Input.GetKeyDown(KeyCode.R))
        {
            CollidersSphere(100);
        }
   
    
    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "ThrowableItem")
        {
            collider.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, liftingForce);
        }
    }

    private void CollidersSphere(float thrust)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, overlapSphereRadius);
               
        foreach (Collider coll in hitColliders)
        {
            if (coll.gameObject.tag == "ThrowableItem")
            {
                Rigidbody rb = coll.gameObject.GetComponent<Rigidbody>();
                rb.AddForce(new Vector3(0, thrust, 0), ForceMode.Impulse);
            }
           
        }
    }

}
