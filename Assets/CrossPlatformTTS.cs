using System.Collections;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class CrossPlatformTTS : MonoBehaviour
{
    public AudioSource audioSource;

    [Header("macOS Voice")]
    public MacVoice macVoice = MacVoice.Samantha;

    [Header("Windows Voice")]
    public WindowsVoice windowsVoice = WindowsVoice.Zira;

    [Header("Testing")]
    public bool testTTSOnStart = false;
    public string testText = "Hello this is a TTS test.";

    string audioPath;

    void Start()
    {
        audioPath = Path.Combine(Application.streamingAssetsPath, "tts.wav");

        // Ensure directory exists
        if (!Directory.Exists(Application.streamingAssetsPath))
        {
            Directory.CreateDirectory(Application.streamingAssetsPath);
        }

        UnityEngine.Debug.Log("TTS Path: " + audioPath);

        if (testTTSOnStart)
        {
            Speak(testText);
        }
    }

    public void Speak(string text)
    {
        UnityEngine.Debug.Log("TTS Code Ran");
        GenerateSpeech(text);
        StartCoroutine(LoadAndPlay());
    }

    void GenerateSpeech(string text)
    {
#if UNITY_STANDALONE_OSX || UNITY_EDITOR_OSX

        string voice = macVoice.ToString();

        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "say";
        psi.Arguments = $"-v {voice} -o \"{audioPath}\" --data-format=LEF32@22050 \"{text}\"";
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        Process process = Process.Start(psi);
        process.WaitForExit();

#elif UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN

        string voice = GetWindowsVoiceName();

        string script = $@"
Add-Type –AssemblyName System.Speech;
$speak = New-Object System.Speech.Synthesis.SpeechSynthesizer;
$speak.SelectVoice('{voice}');
$speak.SetOutputToWaveFile('{audioPath}');
$speak.Speak('{text}');
$speak.Dispose();
";

        string scriptPath = Path.Combine(Application.persistentDataPath, "tts.ps1");
        File.WriteAllText(scriptPath, script);

        ProcessStartInfo psi = new ProcessStartInfo();
        psi.FileName = "powershell";
        psi.Arguments = $"-ExecutionPolicy Bypass -File \"{scriptPath}\"";
        psi.UseShellExecute = false;
        psi.CreateNoWindow = true;

        Process process = Process.Start(psi);
        process.WaitForExit();

#endif
    }

    IEnumerator LoadAndPlay()
    {
        string url = new System.Uri(audioPath).AbsoluteUri;

        UnityEngine.Debug.Log("Loading audio from: " + url);

        using (UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.WAV))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                UnityEngine.Debug.Log("TTS Audio Loaded Successfully");

                AudioClip clip = DownloadHandlerAudioClip.GetContent(www);
                audioSource.clip = clip;
                audioSource.Play();
            }
            else
            {
                UnityEngine.Debug.LogError("TTS Load Failed: " + www.error);
            }
        }
    }

    string GetWindowsVoiceName()
    {
        switch (windowsVoice)
        {
            case WindowsVoice.David:
                return "Microsoft David Desktop";
            case WindowsVoice.Zira:
                return "Microsoft Zira Desktop";
            case WindowsVoice.Mark:
                return "Microsoft Mark Desktop";
            default:
                return "Microsoft Zira Desktop";
        }
    }
}

public enum MacVoice
{
    Samantha,
    Alex,
    Victoria,
    Daniel,
    Karen
}

public enum WindowsVoice
{
    David,
    Zira,
    Mark
}