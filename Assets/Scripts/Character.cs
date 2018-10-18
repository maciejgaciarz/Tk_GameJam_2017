using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

    public int playerNumber = 0;
    public int lifes = 4; //TODO zjebana prywatnosc

    public KeyCode key; //jump
	public ParticleSystem jumpParticles, landParticles;
    public Image JumpImage, ScanImage;

	private float jumpHeight;
	private bool isAlive = true;
	private Animator anim;
	private Rigidbody rb;
	private bool isGrounded = false;
	private bool upwardsPush = false;
	private Vector3 movement = Vector3.zero;
	private float jumpCharge;

    public float jumpCooldown = 3f;

    private float jumpTimer = 0f;

    public void SetJumpTimer()
    {
        JumpImage.fillAmount = jumpTimer < 0 ? 0 : (jumpTimer / jumpCooldown);
    }

    private void Awake()
	{
		rb = GetComponent<Rigidbody>();
		anim = GetComponentInChildren<Animator>();
	}
	
	void Update ()
	{
		if(isAlive)
		{
			//jump
		    //Debug.Log("Jump_" + playerNumber);
			if(isGrounded && jumpTimer < 0 && Input.GetButtonDown("Jump_" + playerNumber))
			{
				//jumpParticles.Play();
				isGrounded = false;
//				anim.SetTrigger("Jump");
				rb.AddForce(Vector3.up * 4f, ForceMode.Impulse);
				jumpCharge = 0f;
				jumpHeight = 0f;
			    jumpTimer = jumpCooldown;
			}
			
			upwardsPush = false;
			if(!isGrounded
				&& Input.GetButtonDown("Jump_" + playerNumber)
                && jumpCharge < .2f)
			{
				jumpCharge += Time.deltaTime;
				upwardsPush = true;
			}
			
//			movement = new Vector3(Input.GetAxis("P"+playerNumber+"Horizontal"), 0f, Input.GetAxis("P"+playerNumber+"Vertical"));
//			movement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));

			//store the jump peak
			if(!isGrounded)
			{
				jumpHeight = Mathf.Max(transform.position.y, jumpHeight);
			}

			//dead condition
			if(transform.position.y < -5f && isAlive)
			{
                    //Debug.Log("Postion below: " + (transform.position.y < -5f) + ", alive: " + isAlive + ", together: " + (transform.position.y < -5f && isAlive));
                isAlive = false;
                GameManager.Instance.OnPlayerDead(playerNumber);
                    //Debug.Log(isAlive);
                    
			}
		}

	    jumpTimer -= Time.deltaTime;
	    SetJumpTimer();

	}

	public void Respawn()
	{
        isAlive = true;

        Vector2 charPos = Random.insideUnitCircle.normalized * 2.5f + Vector2.one * 3.2f;
        transform.position = new Vector3(charPos.x, 1.0f, charPos.y);
        rb.velocity = Vector3.zero;
        isGrounded = false;
    }

	private void FixedUpdate()
	{
//        if (movement.sqrMagnitude > .1f)
//        {
//            rb.AddForce(movement * 20f, ForceMode.Force);
//        }
//
//        if (upwardsPush)
//        {
//            rb.AddForce(Vector3.up * 1f, ForceMode.Force);
//        }
//
//        rb.velocity = new Vector3(rb.velocity.x * 1.8f, rb.velocity.y, rb.velocity.z * 1.8f);
        //anim.SetFloat("Speed", movement.sqrMagnitude);
    }

	private void OnCollisionEnter(Collision coll)
	{
		if(isGrounded)
			return;

		if(coll.gameObject.tag == "Ground"
			&& rb.velocity.y <= 0.1f)
		{
			//landParticles.Play();
			isGrounded = true;
			WaveManager.Instance.CreateWave(new Vector2(transform.position.x, transform.position.z) * 10f, this.playerNumber, .01f);
//			WaveManager.Instance.CreateWave(new Vector2(transform.position.x, transform.position.z), this.playerNumber, jumpHeight * .6f);
			//anim.SetTrigger("Landed");
		}
	}
}
