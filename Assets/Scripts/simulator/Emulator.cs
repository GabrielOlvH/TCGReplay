using System.Collections.Generic;
using compiler;
using simulator;
using TMPro;
using UnityEngine;

public class Emulator : MonoBehaviour
{

    
    public int currentRound = 0;
    private int delay = 0;
    private List<RoundData> _game;
    public CardController _blueController;
    public CardController _redController;
    private string bluePlayer;
    private string redPlayer;
    public TextAsset battleLog;
    public TextMeshProUGUI log;

    private List<DslToken> tokens;

    private bool simulation = false;

    public Simulator simulator;

    private void Start()
    {
      
    }

    private bool isFirstUpdate = true;

    private void FixedUpdate()
    {
        if (isFirstUpdate)
        {
            isFirstUpdate = false;
            _game = new();
            var time = Time.realtimeSinceStartup;
            string txt = battleLog.text;
            redPlayer = "Zzamudio22";
            bluePlayer = "notsteven13";
            tokens = new Tokenizer(bluePlayer, redPlayer).Tokenize(txt);

            simulator = new Simulator
            {
                Simulation = simulation,
                Tokens = tokens,
                StartToken = 0,
                bluePlayer = bluePlayer,
                redPlayer = redPlayer,
                BlueController = _blueController,
                RedController = _redController,
            };
            simulator.Parse();
            
            if (simulation)
            {

                while (simulator.Step()) ;
                Debug.Log("Ended simulation.");
                simulator.Simulation = false;
                simulator.Parse();
            }

            Debug.Log("Parsed in " + (Time.realtimeSinceStartup - time));
        }

        simulator.Step();

    }

}