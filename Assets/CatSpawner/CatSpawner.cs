using UnityEngine;

public class CatSpawner : MonoBehaviour, Interactable
{
    public GameObject catPrefab;
    

    public void Interact(PlayerInteractor interactor)
    {
        float yOffset = 1.5f; 
        Vector3 spawnPosition = transform.position + new Vector3(0, yOffset, 0);
        Quaternion rotation = Quaternion.Euler(90, 0, 0);
        GameObject newCat = Instantiate(catPrefab, spawnPosition, rotation);
    }


}
