using UnityEngine;
using UnityEngine.UI;

public class PlayerData : MonoBehaviour
{
    public string playerName;
    public int playerAge;

    public void SetName(string name)
    {
        playerName = name;
        Debug.Log("Nombre actualizado: " + playerName);
    }

    public void SetAge(string ageStr)
    {
        if (int.TryParse(ageStr, out int result))
        {
            playerAge = result;
            Debug.Log("Edad actualizada: " + playerAge);
        }
        else
        {
            Debug.LogWarning("Edad no válida: " + ageStr);
        }
    }
}
