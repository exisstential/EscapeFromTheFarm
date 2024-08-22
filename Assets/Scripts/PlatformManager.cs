using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformManager : MonoBehaviour
{
    [SerializeField] private GameObject PlatformPrefab = null;

    private List<PlatformScript> PlatformsList = new List<PlatformScript>();
    [SerializeField] private Transform playerTransform = null;
    float LastPlatformLeftMostXPos = 0f;

    [SerializeField] private GameObject mushroomPrefab = null;
    float minPlatY = -4f, maxPlatY = 1f;

    [SerializeField] private List<GameObject> rockPrefabs = new List<GameObject>();
    [SerializeField] private GameObject woodPrefab = null;
    [SerializeField] private GameObject Coins1 = null;

    [SerializeField] private PlayerHealth HealthScript = null;
    [SerializeField] private GameObject HeartPrefab = null;
    int MaxHealth = 3;

    private void Start()
    {
        MaxHealth = HealthScript.GetMaxHealth();

        PlatformScript new_platform = Instantiate(PlatformPrefab, new Vector2(1f, -3f), Quaternion.identity).GetComponent<PlatformScript>();

        new_platform.SetScale(20f);

        PlatformsList.Add(new_platform);

        LastPlatformLeftMostXPos = -9f;
    }
    
    private void Update() 
    {
        if(playerTransform.position.x > LastPlatformLeftMostXPos - 20f)
        {
            CreateNewPlatform();

            float firstPlatRightMostX = PlatformsList[0].GetPosition().x + PlatformsList[0].GetSize() / 2f;
            if (firstPlatRightMostX < playerTransform.position.x - 10f)
            {
                Destroy(PlatformsList[0].gameObject);
                PlatformsList.RemoveAt(0);
            }
        }
    }
    void CreateNewPlatform()
    {
        bool spawnMushroom = Random.value < 0.1f;


        int lastPlatIndex = PlatformsList.Count - 1;
        PlatformScript lastPlatScript = PlatformsList[lastPlatIndex];
        float lastPlatSize = lastPlatScript.GetSize(); 
        Vector2 lastPlatPos = lastPlatScript.GetPosition();
        Vector2 RightMostPosOfLastPlat = new Vector2((lastPlatSize/2) + lastPlatPos.x, lastPlatPos.y);

        float X_space = 0f;
        float Y_diff = 0f;

        int PreviousPlatIndex = 0;
        if (PlatformsList.Count > 1)
        {
            PreviousPlatIndex = PlatformsList.Count - 1;
        }

        if(PlatformsList[PreviousPlatIndex].GetHasMushroom())
        {
            float newPlatY = Random.Range(minPlatY, maxPlatY);
            Y_diff = newPlatY - RightMostPosOfLastPlat.y;

            if (Y_diff >0)
            {
                float distMultiplier = (maxPlatY - minPlatY) - Y_diff;
                X_space = distMultiplier * 2.5f + 5f;
            }
            else
            {
               X_space = (maxPlatY - minPlatY) * 2.5f + 5;
            }
        }
        else
        {
            X_space = Random.Range(2f, 8f);
            Y_diff = Random.Range(-1f, 1f);
        }




        float newPlatSize = Random.Range(3, 13);
        float newXPosition = RightMostPosOfLastPlat.x + X_space + (newPlatSize / 2);
        float newYPosition = RightMostPosOfLastPlat.y + Y_diff;
        newYPosition = Mathf.Clamp(newYPosition, minPlatY, maxPlatY);
        Vector2 NewPlatPos = new Vector2(newXPosition, newYPosition);
        PlatformScript new_platform = Instantiate(PlatformPrefab, NewPlatPos, Quaternion.identity).GetComponent<PlatformScript>();
        new_platform.SetScale(newPlatSize);
        PlatformsList.Add(new_platform);

        LastPlatformLeftMostXPos = NewPlatPos.x - (newPlatSize / 2);

        if (spawnMushroom)
        {
            Vector2 mushroomSpawnPos = new Vector2(NewPlatPos.x + (newPlatSize / 2) - 0.6f, NewPlatPos.y + 0.45f);
            GameObject newMushroom = Instantiate(mushroomPrefab, mushroomSpawnPos, Quaternion.identity, new_platform.transform);
            new_platform.MushroomSpawned();        
        }

        bool spawnRock = Random.value < 0.5f && newPlatSize >= 8f;
        if (spawnRock)
        {
            float min_rockX = newXPosition - newPlatSize / 2f + 3f;
            float max_rockX = newXPosition + newPlatSize / 2f - 3f;
            float rockX = Random.Range(min_rockX, max_rockX);
            Vector2 newRockPos = new Vector2(rockX, newYPosition + 0.45f);

            int newRockIndex = Random.Range(0, rockPrefabs.Count);
            GameObject rockToSpawn = rockPrefabs[newRockIndex];
            GameObject newRock = Instantiate(rockToSpawn, newRockPos, Quaternion.identity, new_platform.transform);
        }

        bool spawnWood = Random.value < 0.2f && !spawnRock && newPlatSize >= 5f;
        if (spawnWood)
        {
            float woodX = newXPosition - newPlatSize / 2f;
            float woodY = 8f;
            Vector2 newWoodPos = new Vector2(woodX, woodY);
            GameObject newWood = Instantiate(woodPrefab, newWoodPos, Quaternion.Euler(0f, 0f, 180f), new_platform.transform);
            float GapBetweenPlatAndWood = Random.Range(3.2f, 4.5f);
            float woodHeight = woodY - newYPosition - GapBetweenPlatAndWood;
            newWood.GetComponent<WoodScript>().SetSize(woodHeight);
        }

        if (!spawnRock && newPlatSize >= 5 && Random.value < 0.5f && !(spawnMushroom && newPlatSize < 7))
        {
            GameObject new_coins = Instantiate(Coins1, new Vector2(newXPosition, newYPosition + 1.1f), Quaternion.identity, new_platform.transform);
        }

        if (Random.value <= 0.025f && HealthScript.GetHealth() < MaxHealth)
        {
            float SpawnX_Offset = newPlatSize / 2f - 0.5f;
            float HeartSpawnX = newXPosition + Random.Range(-SpawnX_Offset, SpawnX_Offset);
            float SpawnY_Offset = 1.1f;
            float HeartSpawnY = newYPosition + SpawnY_Offset;
            Vector2 HeartSpawnPos = new Vector2(HeartSpawnX, HeartSpawnY);

            GameObject new_heart = Instantiate(HeartPrefab, HeartSpawnPos, Quaternion.identity, new_platform.transform);
        }
    }
}
