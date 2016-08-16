using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {
    public GameObject enemyPrefab;
    public float width = 10f;
    public float height = 5f;
    private bool movingRight = true;
    public float speed = 5f;
    public float spawnDelay = 0.5f;
    private float xMax;
    private float xMin;

	// Use this for initialization
	void Start () {

        GetGameBounderies();
        SpawnUntilFull();
       

	}
    void GetGameBounderies ()
    {
        float distanceToCamera = transform.position.z - Camera.main.transform.position.z;
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, distanceToCamera));
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, distanceToCamera));
        xMin = leftEdge.x;
        xMax = rightEdge.x;
    }

    void SpawnUntilFull ()
    {
        Transform freePos = NextFreePosition();
        if (freePos)
        {
            GameObject enemy = Instantiate(enemyPrefab, freePos.position, Quaternion.identity) as GameObject;
            enemy.transform.parent = freePos;
        }
        if (NextFreePosition()) {
            Invoke("SpawnUntilFull", spawnDelay);
        }
    }

    public void OnDrawGizmos ()
    {
        Gizmos.DrawWireCube(transform.position, new Vector3(width, height, 0));
    }
	
	// Update is called once per frame
	void Update () {
        //Move enemy formation to the right, otherwise to the left
        SetFormationMovement();

        if (AllMembersDead())
        {
            SpawnUntilFull();
        }
    }

    void SetFormationMovement ()
    {
        if (movingRight)
        {
            transform.position += Vector3.right * speed * Time.deltaTime;
        }
        else
        {
            transform.position += Vector3.left * speed * Time.deltaTime;
        }

        float rightEdgeOfFormation = transform.position.x + (0.5f * width);
        float leftEdgeOfFormation = transform.position.x - (0.5f * width);

        if (leftEdgeOfFormation < xMin)
        {
            movingRight = true;
        }
        else if (rightEdgeOfFormation > xMax)
        {

            movingRight = false;
        }
    }


    Transform NextFreePosition ()
    {
        foreach(Transform childPositionObj in transform)
        {
            if (childPositionObj.childCount == 0)
            {
                return childPositionObj;
            }

        }
        return null;
    }

    bool AllMembersDead ()
    {
        foreach(Transform childPositionObj in transform)
        {
            if (childPositionObj.childCount > 0)
            {
                return false;
            }
            
        }
        return true;
    }
}
