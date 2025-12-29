using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public GameObject gameObject;
    public Transform transform;
    public Camera camera;
    public float normalZ;
    public float marginX;
    public float marginY;

    public float minSize;
    public GameObject player;
    public Transform playerTransform;
    public GameObject enemy;
    public Transform enemyTransform;

    // Start is called before the first frame update
    void Start()
    {
        transform = gameObject.GetComponent<Transform>();
        playerTransform = player.GetComponent<Transform>();
        enemyTransform = enemy.GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {   
        float playerX = playerTransform.position.x;
        float playerY = playerTransform.position.y;
        float enemyX = enemyTransform.position.x;
        float enemyY = enemyTransform.position.y;
        transform.position = new Vector3((playerX + enemyX) / 2, Mathf.Max((playerY + enemyY) / 2, Mathf.Min(playerY, enemyY) - marginY + camera.orthographicSize), normalZ);
        
        camera.orthographicSize = Mathf.Max(minSize, (transform.position.x - (Mathf.Min(playerX, enemyX) - marginX)) / camera.aspect);
    }
}
