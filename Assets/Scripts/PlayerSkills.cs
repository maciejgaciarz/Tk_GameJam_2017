using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSkills : MonoBehaviour
{
    public int playerNumber = 0;

    [SerializeField] private float cooldown0 = 3f;
    [SerializeField] private float cooldown1 = 5f;
    [SerializeField]
    private float force = 100f;
    [SerializeField]
    private float radius = 100.0f;
    [SerializeField]
    private float liftingForce = 0.5f;
    [SerializeField]
    private float overlapSphereRadius = 20.0f;
    [SerializeField]
    private float thrust = 2000;
    [SerializeField]


    private CapsuleCollider skillMaker;
    private int turningBlocks;//obracanie blkow dla wygladu wip

//    private float skillTimer0 = 0f;
    private float skillTimer1 = 0f;

//    public float GetSkillTimer0()
//    {
//        return skillTimer0 < 0 ? 1 : (1 - (skillTimer0 / cooldown0));
//    }

    public float GetSkillTimer1()
    {
        return skillTimer1 < 0 ? 1 : (1 - (skillTimer1 / cooldown1));
    }

    private void Start()
    {
        skillMaker = GetComponent<CapsuleCollider>();
        //turningBlocks = Random.Range(0, 10);
    }

    private void Update()
    {

        //Debug.Log(skillTimer0);
//        Debug.Log(skillTimer0 + ", " + "Wave0_" + playerNumber.ToString() + " " + Input.GetButtonDown("Wave0_" + playerNumber.ToString()));
//        Debug.Log(skillTimer1 + ", " + "Wave1_" + playerNumber.ToString() + " " + Input.GetButtonDown("Wave1_" + playerNumber.ToString()));

//        if (skillTimer0 < 0f && Input.GetButtonDown("Wave0_" + playerNumber.ToString()))//odpychanie
//        {
//            skillMaker.radius = 100;
//            skillTimer0 = cooldown0;
//            Debug.Log("skill_0 used");
//        }
//        else
//        {
//            skillMaker.radius = 0;
//        }

        if (skillTimer1 < 0f && Input.GetButtonDown("Wave1_" + playerNumber.ToString()))//do góry
        {
            CollidersSphere(100);
            skillTimer1 = cooldown1;
            Debug.Log("skill_1 used");
        }

 //       skillTimer0 = skillTimer0 - Time.deltaTime;
        skillTimer1 = skillTimer1 - Time.deltaTime;

    }

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "ThrowableItem")
        {
            //collider.GetComponent<Rigidbody>().AddExplosionForce(force, transform.position, radius, liftingForce);
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
