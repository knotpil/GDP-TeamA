using UnityEngine;
using System;
using OrderOwner;

namespace OrderOwner //to make the owner struct accessible in other files
{
   public enum Hue
    {
        None,
        VeryDark,
        Dark,
        Medium,
        Light,
        VeryLight
    };

    public enum Weight
    {
        None,
        VeryHeavy,
        Heavy,
        Normal,
        Skinny,
        VerySkinny
    };

    public enum Personality
    {
        None,
        VeryTimid,
        Timid,
        Neutral,
        Feisty,
        VeryFeisty
    };
    public class Order
    {
        public Order() { }
        public Hue h_;
        public Weight w_;
        public Personality p_;
    }
};

public class CustomerOrder : MonoBehaviour
{
    [Header("Checkpoint")]
    public GameObject checkpoint;

    [Header("Placed Order?")]
    public bool placed = false;

    [Header("Order to Place")]
    public Order o = new Order();
    void Start()
    {
        Hue[] hues = (Hue[])Enum.GetValues(typeof(Hue));
        Weight[] weights = (Weight[])Enum.GetValues(typeof(Weight));
        Personality[] personalities = (Personality[])Enum.GetValues(typeof(Personality));

        o.h_ = hues[UnityEngine.Random.Range(1, hues.Length - 1)];
        o.w_ = weights[UnityEngine.Random.Range(1, weights.Length - 1)];
        o.p_ = personalities[UnityEngine.Random.Range(1, personalities.Length - 1)];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CustomerOrderTrigger"))
        {
            checkpoint = other.gameObject;
            checkpoint.GetComponent<Ordering>().customerScript = this;
        }
    }
}

