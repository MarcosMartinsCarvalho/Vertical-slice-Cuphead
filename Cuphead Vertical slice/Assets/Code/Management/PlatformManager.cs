using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab;
    public Transform[] fixedPositions; 
    private List<GameObject> activePlatforms = new List<GameObject>(); 
    private float cycleTime = 5f;

    void Start()
    {
        InitializePlatforms();
        StartCoroutine(PlatformCycle());
    }

    void InitializePlatforms()
    {
        for (int i = 0; i < 4; i++)
        {
            SpawnPlatformAtPosition(i);
        }
    }

    void SpawnPlatformAtPosition(int index)
    {
        GameObject newPlatform = Instantiate(platformPrefab, fixedPositions[index].position, Quaternion.identity);
        activePlatforms.Add(newPlatform);
    }

    IEnumerator PlatformCycle()
    {
        while (true)
        {
            yield return new WaitForSeconds(cycleTime);

            int platformToRemoveIndex = Random.Range(0, activePlatforms.Count);
            GameObject platformToRemove = activePlatforms[platformToRemoveIndex];

            List<int> availablePositions = new List<int>();
            for (int i = 0; i < fixedPositions.Length; i++)
            {
                if (!activePlatforms.Exists(p => p.transform.position == fixedPositions[i].position))
                {
                    availablePositions.Add(i);
                }
            }

            if (availablePositions.Count > 0)
            {
                int newPositionIndex = availablePositions[Random.Range(0, availablePositions.Count)];

                SpawnPlatformAtPosition(newPositionIndex);
            }

            activePlatforms.RemoveAt(platformToRemoveIndex);
            Destroy(platformToRemove);
        }
    }
}
