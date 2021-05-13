using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

[InitializeOnLoad]
public class AutosaveOnRun
{
    static float elapsedTime;
    static readonly float saveTime = 180;

    static bool printSaveWarning = true;
    static bool canSave = true;

    static AutosaveOnRun()
    {
        Debug.Log("Auto save enabled");
        OnEditorUpdate();
    }

    async static void OnEditorUpdate()
    {
        while(true)
        {
            CheckIfAppIsInPlayMode();
            DisableSavingOnPlay();

            if (canSave)
            {
                PrintsSaveWarning();

                elapsedTime++;
                if (elapsedTime >= saveTime)
                {
                    elapsedTime = 0;
                    printSaveWarning = true;

                    if (!EditorApplication.isPlaying)
                    {
                        EditorApplication.SaveScene();
                        Debug.Log($"Saving current scene.... {EditorApplication.currentScene}");
                    }
                    else
                    {
                        Debug.Log("Saving abandon... game is currently playing! Don't save the the duration of playtime.");
                        canSave = false;
                    }
                }

                await Task.Delay(1000);
            }
            else await Task.Delay(1000);
        }
    }

    private static void PrintsSaveWarning()
    {
        if (elapsedTime + 5 > saveTime && printSaveWarning)
        {
            Debug.Log("Saving the game in 5 secs...");
            printSaveWarning = false;
        }
    }

    private static void DisableSavingOnPlay()
    {
        if (EditorApplication.isPlaying && canSave)
        {
            Debug.Log("Saving abandon... game is currently playing! Don't save the the duration of playtime.");
            canSave = false;
        }
    }

    static void CheckIfAppIsInPlayMode()
    {
        if (!EditorApplication.isPlaying && !canSave)
        {
            canSave = true;
            Debug.Log("Autosave resumes");
        }
    }
}