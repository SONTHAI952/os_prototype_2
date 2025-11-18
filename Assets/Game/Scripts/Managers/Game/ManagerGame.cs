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
    [SerializeField] private SOColorTable        gameColorTable;
    [SerializeField] private SOGameFeelsSettings gameFeelsSettings;
    [Header("Controller")]
    [SerializeField] BoardController boardController;
    [SerializeField] SpawnController spawnController;
    [SerializeField] PoolController poolController;
    
    private Camera mainCamera;
    private bool   isPlayable;
    private bool   isPointerOverUI;
    
    public BoardController BoardController => boardController;
    public SpawnController SpawnController => spawnController;
    public PoolController PoolController => poolController;
    
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
