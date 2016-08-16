using UnityEngine;
using System.Collections;

public class EnemyBehavior : MonoBehaviour {

    public GameObject projectile;
    public float health = 150f;
    public float projectileSpeed = 10f;
    public float shotsPerSecond = 0.5f;
    public int scoreValue=150;
    private ScoreKeeper scoreKeeper;
    public AudioClip fireSound;
    public AudioClip deathSound;
    void Start ()
    {
        scoreKeeper = FindObjectOfType<ScoreKeeper>();
    }
	void OnTriggerEnter2D(Collider2D col )
    {
        Projectile missle = col.gameObject.GetComponent<Projectile>();
        if (missle) {
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
        AudioSource.PlayClipAtPoint(deathSound, transform.position);
        scoreKeeper.Score(scoreValue);
        Destroy(gameObject);
    }
    void Fire ()
    {
        Vector3 startPosition = transform.position + new Vector3(0, -1, 0);
        GameObject beam = Instantiate(projectile, startPosition, Quaternion.identity) as GameObject;
        beam.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -projectileSpeed);
        AudioSource.PlayClipAtPoint(fireSound, transform.position);
    }
    void Update ()
    {
        float probability = shotsPerSecond * Time.deltaTime;
        if (Random.value < probability)
            Fire();
    }
}
