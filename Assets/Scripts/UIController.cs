using System.Collections;
using System.Collections.Generic;
using simulator;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    public Emulator emulator;
    public Button speedUpButton;
    public Button slowdownButton;
    public Button pauseButton;
    public Button nextRoundButton;
    public Button prevRoundButton;

    // Start is called before the first frame update
    void Start()
    {
        speedUpButton.onClick.AddListener(OnSpeedupClick);
        slowdownButton.onClick.AddListener(OnSlowdownClick);
        pauseButton.onClick.AddListener(OnTogglePause);
        nextRoundButton.onClick.AddListener(OnNextRoundClick);
        prevRoundButton.onClick.AddListener(OnPrevRoundClick);
    }

    private void OnSpeedupClick() => RoundData.DelayBetweenActions -= 20;

    private void OnSlowdownClick() => RoundData.DelayBetweenActions += 20;

    private void OnTogglePause()
    {
        Simulator.isPaused = !Simulator.isPaused;
        pauseButton.GetComponentInChildren<TextMeshProUGUI>().text = Simulator.isPaused ? "|>" : "||";
    }

    private void OnNextRoundClick()
    {
        Simulator.isPaused = false;
        var sim = emulator.simulator;
        var current = sim.currentRound;
        while (sim.currentRound == current)
        {
            sim.delay = 0;
            sim.Step();
        }

        Simulator.isPaused = true;
    }

    private void OnPrevRoundClick()
    {
        // TODO save snapshots of board for each passed round and rollback
    }
}
