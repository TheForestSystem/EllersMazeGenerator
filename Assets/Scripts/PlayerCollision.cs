using UnityEngine;

public class PlayerCollision : MonoBehaviour
{
    [SerializeField] private Transform playerSpawnPoint;

    private EllersMazeGenerator mazeGenerator;
    private Transform player;

    private void Start()
    {
        mazeGenerator = EllersMazeGenerator.Instance;
        player = GetComponent<Transform>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Goal"))
        {
            player.position = playerSpawnPoint.position;
            mazeGenerator.RegenerateMaze();
            player.position = playerSpawnPoint.position;
        }
    }
}
