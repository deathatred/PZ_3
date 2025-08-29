using UnityEngine;

public class ClearPrefs : MonoBehaviour
{
    public bool clear = false;
    private void Clear()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();
        Debug.Log("PlayerPrefs cleared!");
    }
    private void Update()
    {
        if (clear)
        {
            Clear();
        }
    }
}
