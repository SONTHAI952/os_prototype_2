using CS;
using UnityEngine;
using UnityEngine.Serialization;
using ZeroX.SingletonSystem;

public enum GameResult
{
    None, Win, Lose,
}

public partial class ManagerGame : Singleton_ManualSpawn<ManagerGame>
{
    [Header("Settings")] 
    [SerializeField] private SOLevelTable        levelTable;
    [SerializeField] private SOMapTable        mapTable;
    [SerializeField] private SOGameFeelsSettings gameFeelsSettings;
    [Header("Controller")]
    [SerializeField] BoardController boardController;
    [SerializeField] PlayerController playerController;
    
    private Camera mainCamera;
    private bool   isPlayable;
    private bool   isPointerOverUI;
    
    public BoardController BoardController => boardController;
    public PlayerController PlayerController => playerController;
    public SOGameFeelsSettings GameFeelsSettings => gameFeelsSettings;
    public SOMapTable MapTable => mapTable;
    
    protected void Awake()
    {
        Application.targetFrameRate = 120;
        mainCamera                  = Camera.main;
		
        Awake_Level();
        Awake_Input();
        Awake_Core();
    }
	
    protected void Start()
    {
        Start_Level();
        Start_Input();
        Start_Core();
    }
	
    private void Update()
    {
        Update_Input();
        Update_Core();
    }
}
