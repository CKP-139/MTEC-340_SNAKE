using UnityEngine;

public static class Utilities
{
    public enum GameState
    {
        Play,
        Pause,
        GameOver
    }

    public static float GetNonZeroRandomFloat(float min = -1.0f, float max = 1.0f)
    {
        float num;

        do
        {
            num = Random.Range(min, max);
        } while (Mathf.Approximately(num, 0.0f));

        return num;
    }
    
}
