using System.Collections;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;  // Assign your cherry prefab here in the inspector
    public Transform centerPoint;    // Assign the center point transform here in the inspector
    public float spawnRate = 10f;

    private Camera mainCamera;
    private Vector2 spawnPosition;
    private Vector2 endPosition;

    private void Start()
    {
        mainCamera = Camera.main;
        StartCoroutine(InitialDelay());
    }
    private IEnumerator InitialDelay()
    {
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnCherry());
    }
    private IEnumerator SpawnCherry()
    {
        while (true)
        {
            // Calculate spawn position (just outside of camera view)
            spawnPosition = RandomPositionOutsideCamera();
            GameObject cherryInstance = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

            // Calculate end position (opposite side from spawn position, passing through the center)
            endPosition = (2 * new Vector2(centerPoint.position.x, centerPoint.position.y)) - spawnPosition;

            yield return StartCoroutine(MoveCherry(cherryInstance));  // Wait for the cherry to finish moving

            yield return new WaitForSeconds(spawnRate);  // Then wait for 10 seconds before spawning another
        }
    }


    private Vector2 RandomPositionOutsideCamera()
    {
        Vector2 viewPos;
        Vector2 worldPos;

        // Randomly pick a side: 0 = left, 1 = right, 2 = top, 3 = bottom
        int side = Random.Range(0, 4);

        switch (side)
        {
            case 0:  // Left
                viewPos = new Vector2(-0.1f, Random.Range(0f, 1f));
                break;
            case 1:  // Right
                viewPos = new Vector2(1.1f, Random.Range(0f, 1f));
                break;
            case 2:  // Top
                viewPos = new Vector2(Random.Range(0f, 1f), 1.1f);
                break;
            default:  // Bottom
                viewPos = new Vector2(Random.Range(0f, 1f), -0.1f);
                break;
        }

        worldPos = mainCamera.ViewportToWorldPoint(viewPos);
        return worldPos;
    }

    private IEnumerator MoveCherry(GameObject cherry)
    {
        float t = 0;
        Vector2 startPosition = cherry.transform.position;

        while (t < 1)
        {
            t += Time.deltaTime / spawnRate;  // Adjust this value if the cherry moves too fast/slow
            cherry.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        Destroy(cherry);
    }
}
