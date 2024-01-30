

[System.Serializable]
public class LevelData
{
    public int levelIndex;
    public int starsEarned;

    public LevelData(int levelIndex, int starsEarned)
    {
        this.levelIndex = levelIndex;
        this.starsEarned = starsEarned;
    }
}