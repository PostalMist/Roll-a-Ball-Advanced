using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
    //Variable intialization 
    public float speed;
    public float DEFAULT_SPEED = 10.0f;
    public Text countText;
    public Text WinText;
    private Rigidbody rb;
    private int count;
    private float startTime;
    private float screenStartTime;
    private float boostStartTime;
    private float currentTime;
    private float screenPeriod = 3.0f;
    private float boostPeriod = 8.0f;
    public LayerMask groundLayers;
    public float jumpForce;
    private SphereCollider col;
    private GameObject ground;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<SphereCollider>();
        count = 0;
        SetCountText();
        WinText.text = "";
        startTime = Time.time;
        ground = GameObject.Find("Ground");
        

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        currentTime = Time.time - startTime;
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        rb.AddForce(movement * speed);

        if (IsGrounded() && Input.GetKeyDown(KeyCode.Space)) {

            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
        if (Mathf.Abs( currentTime - boostStartTime) >= boostPeriod)
        {
            speed = DEFAULT_SPEED;
        }
        if (Mathf.Abs(currentTime - screenStartTime) >= screenPeriod)
        {
            WinText.text = "";
        }
	}
    void OnTriggerEnter(Collider other)
    {
        // Destroy(other.gameObject);
        if (other.gameObject.CompareTag("Pick Up"))
        {
            other.gameObject.SetActive(false);
            count++;
            SetCountText();
        } else if (other.gameObject.CompareTag("Pick UpA")) {
            other.gameObject.SetActive(false);
            count += 2;
            SetCountText();
        } else if (other.gameObject.CompareTag("Boost")) {
            other.gameObject.SetActive(false);
            WinText.text = "BOOST";
            screenStartTime = Time.time - startTime;
            speed *= 2;
            boostStartTime = Time.time - startTime;
        }
    }

    void SetCountText() {
        countText.text = "Count: " + count.ToString();
        if (count == 12)
        {
            WinText.text = "Start";
            ground.gameObject.SetActive(false);
            screenStartTime = Time.time - startTime;
        }
        else if (count == 14)
        {
            WinText.text = "";
        }
        else if (count % 2 == 0)
        {
            WinText.text = "Collected " + count.ToString();
            screenStartTime = Time.time - startTime;

        }
        else {
            WinText.text = "";
        }
    }

    private bool IsGrounded() {

        return Physics.CheckCapsule(col.bounds.center, new Vector3(col.bounds.center.x, col.bounds.min.y, col.bounds.center.z), col.radius * 0.9f, groundLayers );
    }
}
