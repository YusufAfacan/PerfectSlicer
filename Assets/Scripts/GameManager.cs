using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GameAnalyticsSDK;
using Facebook.Unity;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;


public class GameManager : MonoBehaviour
{
    public Transform foodSpawnPos;

    public GameObject levelSelectObjects;
    public GameObject levelObjects;
    public GameObject levelEndObjects;

    public GameObject[] foodPrefabs;
    private GameObject nextFood;

    [HideInInspector]
    public int targetWeight;
    private int targetWeightMin;
    private int targetWeightMax;

    public int totalFinishedLevelNumber;

   
    [HideInInspector]
    public int currentFoodNumber;
    private int levelDifficulty;
    private int numberOfFoodsToSpawn;

    [HideInInspector]
    public int tolerance;
    private bool ReducingTolerance = false;

    [HideInInspector]
    public bool lastFood;


    public float money;
    public float moneyMultiplier;
    public int perfectSlices;
    public int goodSlices;
    public int badSlices;

    [Header("UI Elements")]
    public Text targetWeightText;
    public Text perfectSlicesText;
    public Text goodSlicesText;
    public Text badSlicesText;
    public Text moneyAmountText;

    public GameObject NormalTable;
    public GameObject HardTable;
    public GameObject NormalButton;
    public GameObject HardButton;
    public GameObject UnlockNormalButton;
    public GameObject UnlockHardButton;
    public GameObject notEnoughMoney;

    public bool isNormalModeUnlocked = false;
    public bool isHardModeUnlocked = false;



    private void Awake()
    {
        GameAnalytics.Initialize();
    }

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(Application.persistentDataPath);

        Camera.main.orthographic = true;
        tolerance = 8;
        LoadGameManager();

        if (isNormalModeUnlocked)
        {
            NormalTable.SetActive(true);
            NormalButton.SetActive(true);
            UnlockNormalButton.SetActive(false);
            UnlockHardButton.SetActive(true);
        }

        if (isHardModeUnlocked)
        {
            HardTable.SetActive(true);
            HardButton.SetActive(true);
            UnlockHardButton.SetActive(false);
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (currentFoodNumber < numberOfFoodsToSpawn)
        {
            lastFood = false;
        }
        else
        {
            lastFood = true;
        }

        if (nextFood != null && nextFood.transform.position.x <= 0 && !ReducingTolerance)
        {
           ReducingTolerance = true;
           InvokeRepeating(nameof(ReduceTolerance), 1, 1);
        }

        targetWeightText.text = targetWeight.ToString() + " ± " + tolerance;
        moneyAmountText.text = money.ToString();

        
    }

    public void SetTolerance()
    {
        tolerance = 8;
    }

    private void ReduceTolerance()
    {
        tolerance--;
    }

    public void BringNextFood()
    {
        nextFood = foodPrefabs[Random.Range(0, 26)];
        SetTolerance();

        if (nextFood.CompareTag("Avocado"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 90, 270));
        }

        else if (nextFood.CompareTag("Sausage"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 0, 270));
        }

        else if (nextFood.CompareTag("Cheese"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 90, 270));
        }

        else if (nextFood.CompareTag("Chicken"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 0, 90));
        }

        else if (nextFood.CompareTag("Steak"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 270, 270));
        }

        else if (nextFood.CompareTag("Watermelon"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 180, 0));
        }

        else if (nextFood.CompareTag("Banana"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 0, 315));
        }

        else if (nextFood.CompareTag("Pea"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 0, 90));
        }

        else if (nextFood.CompareTag("Bread"))
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.Euler(0, 0, 90));
        }

        else
        {
            Instantiate(nextFood, foodSpawnPos.position, Quaternion.identity);
        }

        currentFoodNumber++;
        targetWeight = Random.Range(targetWeightMin, targetWeightMax);
 
    }
    public void FinishLevel()
    {
        GameAnalytics.NewProgressionEvent(GAProgressionStatus.Complete, totalFinishedLevelNumber.ToString());
        totalFinishedLevelNumber++;
        StartCoroutine(nameof(OpenLevelEndScreen));

        if (money < 0)
        {
            money = 0;
        }

        SaveGameManager();

    }

    public IEnumerator OpenLevelEndScreen()
    {
        yield return new WaitForSeconds(2f);

        levelObjects.SetActive(false);
        levelEndObjects.SetActive(true);

        perfectSlicesText.text = "Perfect Slices: " + perfectSlices.ToString();
        goodSlicesText.text = "Good Slices: " + goodSlices.ToString();
        badSlicesText.text = "Bad Slices: " + badSlices.ToString();

        
    }

    public void ReturnLevelSelect()
    {
        Camera.main.transform.SetPositionAndRotation(new Vector3(0, 5, -15), Quaternion.Euler(18, 0, 0));
        levelSelectObjects.SetActive(true);
       
        if (isNormalModeUnlocked)
        {
            UnlockNormalButton.SetActive(false);
        }

        if (isHardModeUnlocked)
        {
            UnlockHardButton.SetActive(false);
        }


        levelEndObjects.SetActive(false);
        lastFood = false;
        currentFoodNumber = 1;
        perfectSlices = 0;
        goodSlices = 0;
        badSlices = 0;

    }

    public void StartEasyLevel()
    {
        levelDifficulty = 0;
        moneyMultiplier = 1;
        InitializeLevel();
    }

    public void StartNormalLevel()
    {
        levelDifficulty = 1;
        moneyMultiplier = 2;
        InitializeLevel();
    }

    public void StartHardLevel()
    {
        levelDifficulty = 2;
        moneyMultiplier = 3;
        InitializeLevel();
    }

    private void InitializeLevel()
    {
        if (levelDifficulty == 0)
        {
            targetWeightMin = 40;
            targetWeightMax = 60;
            
        }

        if (levelDifficulty == 1)
        {
            targetWeightMin = 30;
            targetWeightMax = 70;
            
        }

        if (levelDifficulty == 2)
        {
            targetWeightMin = 20;
            targetWeightMax = 80;
            
        }

        numberOfFoodsToSpawn = Random.Range(10, 13);
        levelSelectObjects.SetActive(false);
        levelObjects.SetActive(true);
        lastFood = false;
        currentFoodNumber = 1;
        Camera.main.orthographic = true;
        Camera.main.transform.SetPositionAndRotation(new Vector3(0, 0, -10), Quaternion.identity);

        BringNextFood();
    }

    public void SaveGameManager()
    {
        SaveSystem.SaveGameManager(this);
    }

    public void LoadGameManager()
    {
        PlayerData data = SaveSystem.LoadGameManager();
        money = data.money;
        totalFinishedLevelNumber = data.totalFinishedLevelNumber;
        isNormalModeUnlocked = data.isNormalModeUnlocked;
        isHardModeUnlocked = data.isHardModeUnlocked;
    }

    public void UnlockNormalLevel()
    {
        if (money >= 200)
        {
            money -= 200;
            NormalTable.SetActive(true);
            NormalButton.SetActive(true);
            UnlockNormalButton.SetActive(false);
            UnlockHardButton.SetActive(true);
            isNormalModeUnlocked = true;
            SaveGameManager();
        }
        else
        {
            notEnoughMoney.SetActive(true);
            StartCoroutine(nameof(WaitOneSec));
        }
 
    }

    public void UnlockHardLevel()
    {
        if (money >= 400)
        {
            money -= 400;
            HardTable.SetActive(true);
            HardButton.SetActive(true);
            UnlockHardButton.SetActive(false);
            isHardModeUnlocked = true;
            SaveGameManager();
        }
        else
        {
            notEnoughMoney.SetActive(true);
            StartCoroutine(nameof(WaitOneSec));
        }
    }

    IEnumerator WaitOneSec()
    {
        yield return new WaitForSeconds(1);
        notEnoughMoney.SetActive(false);
    }
}

