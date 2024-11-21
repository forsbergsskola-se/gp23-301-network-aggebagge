using UnityEngine;

public class PlayerPrefsData : MonoBehaviour
{
    private const string IsRoomHostKey = "IsRoomHost";
    private const string RoomCodeKey = "RoomCodeKey";
    
    // Singleton instance
    public static PlayerPrefsData i { get; private set; }
    
    private void Awake()
    {
        // Check if an instance already exists
        if (i != null && i != this)
        {
            Destroy(gameObject); // Destroy duplicate instance
            return;
        }
        i = this;
        DontDestroyOnLoad(gameObject); // Make this object persistent across scenes
    }

    public void SavePrefsData(bool isHost, string code)
    {
        PlayerPrefs.SetString(RoomCodeKey, code);
        PlayerPrefs.SetInt(IsRoomHostKey, isHost ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool IsHost()
    {
        int isEnabledInt = PlayerPrefs.GetInt(IsRoomHostKey, 1);
        bool isEnabled = isEnabledInt == 1;
        return isEnabled;
    }
    
    public string GetCode()
    {
        string roomCode = PlayerPrefs.GetString(RoomCodeKey, "");
        return roomCode;
    }
    

    // private void Start()
    // {
    //     // Example usage
    //     SavePrefsData(false, "");
    //     bool soundEnabled = LoadSoundPreference();
    //     Debug.Log("Is sound enabled: " + soundEnabled);
    // }
}