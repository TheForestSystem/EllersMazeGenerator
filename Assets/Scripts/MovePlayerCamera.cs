using UnityEngine;

public class MovePlayerCamera : MonoBehaviour
{
    [SerializeField]
    private Transform playerCameraPos;

    // Update is called once per frame
    void Update()
    {
        transform.position = playerCameraPos.position;
    }
}
