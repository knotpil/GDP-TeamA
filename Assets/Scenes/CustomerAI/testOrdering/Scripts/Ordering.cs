using System.Collections.Generic;
using UnityEngine;
using OrderOwner;

public class Ordering : MonoBehaviour
{
    [Header("Necessary Components")]
    public PlayerTrigger player;
    public CustomerOrder customerScript;

    [Header("Current Orders")]
    public Queue<Order> orderQueue = new Queue<Order>();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && customerScript && player.checkpointTrigger)
        {
            Debug.Log("Success!");
            if (!customerScript.placed)
            {
                orderQueue.Enqueue(customerScript.o);
                customerScript.placed = true;
            }
            Debug.Log(orderQueue.Peek().r_ + orderQueue.Peek().g_ + orderQueue.Peek().b_);
            Debug.Log(orderQueue.Peek().w_);
            Debug.Log(orderQueue.Peek().hl_);
            Debug.Log(orderQueue.Peek().s_);
            Debug.Log(orderQueue.Peek().pattern_);
            Debug.Log(orderQueue.Peek().p_);
        }
    }
}


