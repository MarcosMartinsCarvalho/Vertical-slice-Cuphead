using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    public GameObject platformPrefab; // Prefab de la plataforma
    public Transform[] fixedPositions; // Posiciones fijas
    private List<GameObject> activePlatforms = new List<GameObject>(); // Plataformas activas
    private float cycleTime = 5f; // Tiempo entre ciclos

    void Start()
    {
        InitializePlatforms();
        StartCoroutine(PlatformCycle());
    }

    void InitializePlatforms()
    {
        // Activar 4 plataformas al inicio
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

            // Selecciona una plataforma aleatoria para desaparecer
            int platformToRemoveIndex = Random.Range(0, activePlatforms.Count);
            GameObject platformToRemove = activePlatforms[platformToRemoveIndex];

            // Encuentra una nueva posición que no esté siendo usada
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

                // Crear nueva plataforma en la nueva posición
                SpawnPlatformAtPosition(newPositionIndex);
            }

            // Elimina la plataforma vieja
            activePlatforms.RemoveAt(platformToRemoveIndex);
            Destroy(platformToRemove);
        }
    }
}
