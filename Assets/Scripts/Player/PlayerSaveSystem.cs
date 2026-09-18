using System.IO;
using UnityEngine;

public class PlayerSaveSystem : MonoBehaviour
{
    private const string SaveFileName = "player_save.csv";

    private string SavePath => Path.Combine(
        Application.persistentDataPath,
        SaveFileName
    );

    public void SaveKillCount(int killCount)
    {
        File.WriteAllText(SavePath, $"KillCount,{killCount}");
    }

    public int LoadKillCount()
    {

        if (!File.Exists(SavePath))
            return 0;

        string csv = File.ReadAllText(SavePath);

        string[] values = csv.Split(',');


        if (values.Length >= 2 &&
            values[0] == "KillCount" &&
            int.TryParse(values[1], out int killCount))
        {
            return killCount;
        }

        return 0;
    }
}