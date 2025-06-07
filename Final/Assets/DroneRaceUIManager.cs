using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DroneRaceUIManager : MonoBehaviour
{
    public DroneSteeringController droneController;
    public Button startButton;
    public TextMeshProUGUI countdownText;
    public TextMeshProUGUI stopwatchText;

    private float stopwatchTime = 0f;
    private bool stopwatchRunning = false;

    void Start()
    {
        // Disable drone control at startup
        droneController.enabled = false;
        countdownText.text = "";
        stopwatchText.text = "";
        startButton.onClick.AddListener(OnStartPressed);
    }

    void Update()
    {
        if (stopwatchRunning)
        {
            stopwatchTime += Time.deltaTime;
            stopwatchText.text = stopwatchTime.ToString("F2") + "s";
        }
    }

    void OnStartPressed()
    {
        startButton.gameObject.SetActive(false);
        StartCoroutine(CountdownAndStart());
    }

    IEnumerator CountdownAndStart()
    {
        int count = 3;
        while (count > 0)
        {
            countdownText.text = count.ToString();
            yield return new WaitForSeconds(1f);
            count--;
        }

        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.text = "";

        droneController.enabled = true;
        stopwatchRunning = true;
        stopwatchTime = 0f;
    }

    // Optional: call this externally when course is completed
    public void StopStopwatch()
    {
        stopwatchRunning = false;
    }

    public float GetFinalTime()
    {
        return stopwatchTime;
    }
}