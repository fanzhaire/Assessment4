using System.Collections;
using UnityEngine;

public class CherryController : MonoBehaviour
{
    public GameObject cherryPrefab;
    public Transform centerPoint;
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
        // Calculate spawn position (just outside of camera view)
        spawnPosition = RandomPositionOutsideCamera();
        GameObject cherryInstance = Instantiate(cherryPrefab, spawnPosition, Quaternion.identity);

        // Calculate end position (opposite side from spawn position, passing through the center)
        endPosition = (2 * new Vector2(centerPoint.position.x, centerPoint.position.y)) - spawnPosition;

        yield return StartCoroutine(MoveCherry(cherryInstance));

        // After destroying the cherry, wait for 10 seconds before spawning another
        yield return new WaitForSeconds(spawnRate);
        StartCoroutine(SpawnCherry());
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
            if (cherry == null) yield break;  // Check if the cherry has been destroyed

            t += Time.deltaTime / spawnRate;
            cherry.transform.position = Vector2.Lerp(startPosition, endPosition, t);
            yield return null;
        }

        if (cherry != null) Destroy(cherry);  // Check again before destroying
    }
}
