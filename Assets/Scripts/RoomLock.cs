using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    // Config Params
    [SerializeField] Sprite closedGateSideSprite;
    [SerializeField] Sprite closedGateSideBigSprite;
    [SerializeField] Sprite closedGateFrontSprite;
    [SerializeField] Sprite openGateSideSprite;
    [SerializeField] Sprite openGateSideBigSprite;
    [SerializeField] Sprite openGateFrontSprite;
    [SerializeField] GameObject closedGateSide;
    [SerializeField] GameObject closedGateSideBig;
    [SerializeField] GameObject closedGateUp;
    [SerializeField] GameObject closedGateDown;
    [SerializeField] GatesConfig gatesConfig;

    // Dynamic Variables
    List<GameObject> gates;
    [SerializeField] int spawnWaveCount;
    bool locked = false;

    // Start is called before the first frame update
    void Start()
    {
        gates = new List<GameObject>();
        GatesInitializaiton();
    }

    // Update is called once per frame
    void Update()
    {
        WaveCountChecker();
    }

    private void WaveCountChecker()
    {
        if (spawnWaveCount == 0)
        {
            GateOpen();
            locked = false;
        }
    }

    void GatesInitializaiton()
    {
        var tags = gatesConfig.GetGateTags();
        var gatePositions = gatesConfig.GetGatePositions();
        for(int i  = 0; i < gatePositions.Count; i++)
        {
            if (tags[i] == "Side_Gate")
            {
                GameObject gate = Instantiate(closedGateSide, gatePositions[i].transform.position, Quaternion.identity);
                gate.tag = "Side_Gate";
                gate.GetComponent<Collider2D>().enabled = false;
                gates.Add(gate);
            } else if (tags[i] == "Up_Gate")
            {
                GameObject gate = Instantiate(closedGateUp, gatePositions[i].transform.position, Quaternion.identity);
                gate.GetComponent<Collider2D>().enabled = false;
                gates.Add(gate);
            } else if (tags[i] == "Down_Gate")
            {
                GameObject gate = Instantiate(closedGateDown, gatePositions[i].transform.position, Quaternion.identity);
                gate.GetComponent<Collider2D>().enabled = false;
                gates.Add(gate);
            } else if (tags[i] == "SideBig_Gate")
            {
                GameObject gate = Instantiate(closedGateSideBig, gatePositions[i].transform.position, Quaternion.identity);
                gate.tag = "SideBig_Gate";
                gate.GetComponent<Collider2D>().enabled = false;
                gates.Add(gate);
            }
        }
    }

    void GateClose()
    {
        for (int i = 0; i < gates.Count; i++)
        {
            GameObject gate = gates[i];
            if (gate.tag == "Side_Gate") 
            {
                gate.GetComponent<SpriteRenderer>().sprite = closedGateSideSprite;
            } else if (gate.tag == "SideBig_Gate")
            {
                gate.GetComponent<SpriteRenderer>().sprite = closedGateSideBigSprite;
            }
            else
            {
                gate.GetComponent<SpriteRenderer>().sprite = closedGateFrontSprite;
            }
            gate.GetComponent<Collider2D>().enabled = true;
        }
    }

    void GateOpen()
    {
        for (int i = 0; i < gates.Count; i++)
        {
            GameObject gate = gates[i];
            if (gate.tag == "Side_Gate")
            {
                gate.GetComponent<SpriteRenderer>().sprite = openGateSideSprite;
            } else if (gate.tag == "SideBig_Gate")
            {
                gate.GetComponent<SpriteRenderer>().sprite = openGateSideBigSprite;
            } else
            {
                gate.GetComponent<SpriteRenderer>().sprite = openGateFrontSprite;
            }
            gate.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !locked)
        {
            GateClose();
            locked = true;
        }
    }

    public void NewWaveSpawn()
    {
        spawnWaveCount--;
    }

    public bool ReturnLockedState()
    {
        return locked;
    }

    public void SetSpawnCounts(int spawnCount)
    {
        spawnWaveCount = spawnCount;
    }
}
