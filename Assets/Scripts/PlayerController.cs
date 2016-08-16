using UnityEngine;
using System.Collections;


public class PlayerController : MonoBehaviour {

    public float speed=10f;
    public float padding = 1f;
    public GameObject laser;
    public float laserSpeed;
    public float firingRate = 0.2f;
    float xMin;
    float xMax;
    public float health = 250f;
    public AudioClip fireSound;
	void Start () {

        ConfigureCameraBoundaries();

    }

    void ConfigureCameraBoundaries ()
    {
        //Get camera distance relative to object
        float distance = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftMost = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distance));
        Vector3 rightMost = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distance));
        //Set min and max positions relative to camera, not screen size
        xMin = leftMost.x + padding;
        xMax = rightMost.x - padding;
    }

    void Fire ()
    {
        Vector3 startPosition = transform.position + new Vector3(0, 1, 0);
        GameObject beam = Instantiate(laser, startPosition, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector3(0, laserSpeed, 0);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
    // Update is called once per frame
    void Update () {

        //TODO add win condition


        //Space Key will Shoot Laser
        if (Input.GetKeyDown(KeyCode.Space)) {
            InvokeRepeating("Fire", 0.00001f, firingRate);
        } if (Input.GetKeyUp(KeyCode.Space))
        {
            CancelInvoke("Fire");
        }

        //Check for left & right arrow key presses for Movement
        if (Input.GetKey(KeyCode.LeftArrow)) {
            transform.position += Vector3.left * speed * Time.deltaTime;

        }
        else if (Input.GetKey(KeyCode.RightArrow)) {

            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        RestrictPlayerMovement();
    }

    void RestrictPlayerMovement ()
    {
        //Restricts players x position to the playspace
        float clampedXPos = Mathf.Clamp(transform.position.x, xMin, xMax);
        this.transform.position = new Vector3(clampedXPos, transform.position.y, transform.position.z);
    }

    void OnTriggerEnter2D ( Collider2D col )
    {
        Projectile missle = col.gameObject.GetComponent<Projectile>();
        if (missle)
        {
            missle.Hit();
            health -= missle.GetDamage();
            if (health <= 0)
            {
                Die();
            }

        }

    }

    void Die ()
    {
        LevelManager levelManager = FindObjectOfType<LevelManager>();
        Destroy(gameObject);
        levelManager.LoadLevel("Lose Screen");
    }
}
