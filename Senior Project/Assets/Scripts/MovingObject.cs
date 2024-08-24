using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem.Controls;

public class MovingObject : MonoBehaviour
{
    public Item item;
    public GameObject target;
    public GameObject hand;
    public GameObject oriPos;

    public float speed;
    
    void Start()
    {

    }

    void Update()
    {
        if(item.targetSpawn == true)
        {
            hand.transform.position = Vector3.MoveTowards(hand.transform.position , target.transform.position , speed * Time.deltaTime);
        }

        if(item.targetSpawn == false)
        {
            hand.transform.position = Vector3.MoveTowards(hand.transform.position , oriPos.transform.position, speed * Time.deltaTime);
        }
    }

    // Start is called before the first frame update
    /*void Start()
    {
        spawnTime = Random.Range(10,15);
        hand.transform.position = oriPos.transform.position;
    }

    // Update is called once per frame
    void Update()
    {   
        for(int i = 8 ; i > 0; i--)
        {
            if(spawnTime > 0)
            {
                targetSpawn = false;
            }

            if(targetSpawn == false)
            {
                targetSpawn = true;
            }
        }

        if(targetSpawn == true)
        {
            hand.transform.position = Vector3.MoveTowards(hand.transform.position , target.transform.position , speed);
        }

        if(targetSpawn == false)
        {
            hand.transform.position = Vector3.MoveTowards(hand.transform.position , oriPos.transform.position, speed);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            Destroy(target.gameObject);
            targetSpawn = false;
        }
    }

    void SpawnItem()
    {
        GameObject f = Instantiate(target) as GameObject;
    }

    IEnumerator itemSpawn()
    {
        while(true)
        {
            yield return new WaitForSeconds(spawnTime);
            SpawnItem();
        }
    }*/
}
