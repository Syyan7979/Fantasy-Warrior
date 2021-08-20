using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomLock : MonoBehaviour
{
    // Config Params
    [SerializeField] Sprite closedGateSideSprite;
    [SerializeField] Sprite closedGateFrontSprite;
    [SerializeField] Sprite openGateSideSprite;
    [SerializeField] Sprite openGateFrontSprite;
    [SerializeField] GameObject closedGateSide;
    [SerializeField] GameObject closedGateUp;
    [SerializeField] GameObject closedGateDown;
    [SerializeField] GatesConfig gatesConfig;
    [SerializeField] int spawnWaveCount;

    // Dynamic Variables
    List<GameObject> gates;
    
    // Start is called before the first frame update
    void Start()
    {
        gates = new List<GameObject>();
        spawnWaveCount = FindObjectOfType<EnemySpawner>().GetSpawnWaveCount();
        GatesInitializaiton();
    }

    // Update is called once per frame
    void Update()
    {
        if (spawnWaveCount == 0)
        {
            GateOpen();
        }
    }

    void GatesInitializaiton()
    {
        var tags = gatesConfig.GetGateTags();
        var gatePositions = gatesConfig.GetGatePositions();
        GameObject gate = new GameObject();
        for(int i  = 0; i < gatePositions.Count; i++)
        {
            if (tags[i] == "Side_Gate")
            {
                gate = Instantiate(closedGateSide, gatePositions[i].transform.position, Quaternion.identity);
                gate.tag = "Side_Gate";
            } else if (tags[i] == "Up_Gate")
            {
                gate = Instantiate(closedGateUp, gatePositions[i].transform.position, Quaternion.identity);
            } else if (tags[i] == "Down_Gate")
            {
                gate = Instantiate(closedGateDown, gatePositions[i].transform.position, Quaternion.identity);
            }
            gate.GetComponent<Collider2D>().enabled = false;
            gates.Add(gate);
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
            } else
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
            }
            else
            {
                gate.GetComponent<SpriteRenderer>().sprite = openGateFrontSprite;
            }
            gate.GetComponent<Collider2D>().enabled = false;
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            GateClose();
        }
    }

    public void NewWaveSpawn()
    {
        spawnWaveCount--;
    }
}
