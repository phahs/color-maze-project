using UnityEngine;
using UnityEngine.UI;

public class Game_Manager : MonoBehaviour
{
    public Map mapPrefab;
    public Player pcPrefab;
    public Canvas mainMenu;
    public Canvas playerUI;
    public InputField testLevel;
    public Camera menuCamera;
    
    private bool gameStart;
    private int level;
    private Map mapInstance;
    
    void Start()
    {
        level = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameStart)
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                RestartGame();
            }
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                EndGame();
            }
        }
    }

    public void BeginGame()
    {
        gameStart = true;
        if(testLevel.text == "")
        {
            setLevel(1);
        }
        else
        {
            setLevel(int.Parse(testLevel.text));
        }
        
        mapInstance = Instantiate(mapPrefab) as Map;
        mapInstance.GenerateMap(level);
    }

    public void setLevel(int value)
    {

        level = value;
    }

    public void RestartGame()
    {
        Destroy(mapInstance.gameObject);

        BeginGame();
    }

    public void EndGame()
    {
        gameStart = false;
        Destroy(mapInstance.gameObject);
        playerUI.gameObject.SetActive(false);
        menuCamera.gameObject.SetActive(true);
        mainMenu.gameObject.SetActive(true);
    }

    public void MenuQuit()
    {
        Application.Quit();
    }
}
