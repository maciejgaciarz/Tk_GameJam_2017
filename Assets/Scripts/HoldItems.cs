using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoldItems : MonoBehaviour
{
        public int playerNumber = 0;
		public float speed = 10;
		public bool canHold = true;
		public GameObject ball;
		public Transform guide;
    private Animator rabbitAnimator;

    void Awake()
    {
        rabbitAnimator = GetComponentInParent<Animator>();
    }

		void Update()
		{
		    if (Input.GetButtonDown("Grab_" + playerNumber)) 
			{
            if (!canHold)
            {
                throw_drop();
                rabbitAnimator.SetBool("GrabObject", false);
            }
            else
            {
                Pickup();
                rabbitAnimator.SetBool("GrabObject", true);
            }
			}//mause If

			if (!canHold && ball)
				ball.transform.position = guide.position;

		}//update

		//We can use trigger or Collision
		void OnTriggerEnter(Collider col)
		{
            if (col.gameObject.tag == "ThrowableItem" )
            {
                if (!ball) // if we don't have anything holding
                {
                    ball = col.gameObject;
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                }

            }

            if (col.gameObject.tag == "Player")
            {

                if (!ball) // if we don't have anything holding
                {

                ball = col.gameObject;
                //col.gameObject.GetComponent<Rigidbody>().constraints
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
                }
            }
    }

		//We can use trigger or Collision
		void OnTriggerExit(Collider col)
		{
		if (col.gameObject.tag == "ThrowableItem" || col.gameObject.tag == "Player")
			{
                if (canHold)
                {
                col.gameObject.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
                ball = null;
                }

        }
		    
    }
        

		private void Pickup()
		{
        //ball.GetComponent<Rigidbody>().isKinematic = true;
        if (!ball)
        {
            return;
        }
		    if (ball.gameObject.tag == "Player")
		    {
                ball.GetComponent<HoldItems>().enabled = false;
            }
        //m_animator.SetBool("Grounded", m_isGrounded);
        //We set the object parent to our guide empty object.
        ball.transform.SetParent(guide);
    	//Set gravity to false while holding it
		ball.GetComponent<Rigidbody>().useGravity = false;
    	//we apply the same rotation our main object (Camera) has.
		ball.transform.localRotation = transform.rotation;
        if (ball.gameObject.tag == "Player")
        {
            ball.transform.Rotate(0, 0, 90);           
        }
        //We re-position the ball on our guide object 
        ball.transform.position = guide.position;          
		canHold = false;
         }

    private void throw_drop()
		{
			if (!ball)
				return;

			//Set our Gravity to true again.
			ball.GetComponent<Rigidbody>().useGravity = true;
            if (ball.gameObject.tag == "Player")
            {
                ball.GetComponent<HoldItems>().enabled = true;
            }
        // we don't have anything to do with our ball field anymore
        ball = null; 
			//Apply velocity on throwing
			guide.GetChild(0).gameObject.GetComponent<Rigidbody>().velocity = transform.forward * speed;
		    //ball.GetComponent<Rigidbody>().isKinematic = false;
			//Unparent our ball
			guide.GetChild(0).parent = null;
			canHold = true;
		}
}//class
