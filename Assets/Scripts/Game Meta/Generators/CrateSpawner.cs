using System.Collections;
using UnityEngine;

public class CrateSpawner : MonoBehaviour
{
    public GameObject healthCrate;
    public float spawnDelayMin;
    public float spawnDelayMax;
    public float spawnRange;
    public float spawnCheckRadius;

    [Space]
    public string toastMessage;
    public Color toastColor;
    public string toastID;

    float nextSpawnTime;

    private void OnEnable()
    {
        StartCoroutine(SpawnCrateRoutine());
    }

    private IEnumerator SpawnCrateRoutine()
    {
        yield return new WaitForSeconds(Random.Range(spawnDelayMin, spawnDelayMax));

        Vector2 spawnLocation = Util.GetSpawnLocation(spawnRange, spawnCheckRadius);
        Instantiate(healthCrate, spawnLocation, Quaternion.identity);

        Toast.DisplayMessage(new Toast.ToastData(toastID, toastMessage, toastColor));

        StartCoroutine(SpawnCrateRoutine());
    }
}
