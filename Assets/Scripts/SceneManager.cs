using UnityEngine;

public class SceneManager : Singleton<SceneManager>
{

    public Player Player;
    public EnemiesManager EnemiesManager = new();
    public GameObject Lose;
    public GameObject Win;

    private int currWave = 0;
    [SerializeField] private LevelConfig Config;

    private void Start()
    {
        SpawnWave();
    }

    public void AddEnemie(Enemie enemie)
    {
        EnemiesManager.AddEnemy(enemie);
    }

    public void RemoveEnemie(Enemie enemie)
    {
        EnemiesManager.RemoveEnemy(enemie);
        Debug.Log(EnemiesManager.Enemies.Count);
        if(EnemiesManager.Enemies.Count == 0)
        {
            SpawnWave();
        }
    }

    public void GameOver()
    {
        Lose.SetActive(true);
    }

    private void SpawnWave()
    {
        Debug.Log(currWave + " - " + Config.Waves.Length);
        if (currWave >= Config.Waves.Length)
        {
            Win.SetActive(true);
            return;
        }

        var wave = Config.Waves[currWave];
        foreach (var character in wave.Characters)
        {
            for (var i = 0; i < character.Count; i++)
            {
                Vector3 pos = new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10));
                Instantiate(character.Prefab, pos, Quaternion.identity);
            }
        }
        currWave++;
    }

    public void Reset()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }
    

}
