using UnityEngine;
using System;
using OrderOwner;

namespace OrderOwner //to make the owner struct accessible in other files
{
    public enum HairLength 
    {
        None,
        //Bald, //commented out since it is a suggestion
        VeryShort,
        Short,
        Medium,
        Long,
        VeryLong
    };

    public enum Weight
    {
        None,
        VerySkinny,
        Skinny,
        Normal,
        Heavy,
        VeryHeavy
    };

    public enum Shine
    {
        None,
        NotShiny,
        LittleShiny,
        Shiny,
        VeryShiny
    };

    public enum Pattern
    {
        Unassigned,
        None,
        Spots,
        Stripes,
        Calico
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
        public Weight w_;
        public HairLength hl_;
        public Shine s_;
        public Pattern pattern_;
        public Personality p_;
        public int r_, g_, b_; //color values
    }
}

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
        Weight[] weights = (Weight[])Enum.GetValues(typeof(Weight));
        HairLength[] hairLengths = (HairLength[])Enum.GetValues(typeof(HairLength));
        Shine[] shines = (Shine[])Enum.GetValues(typeof(Shine));
        Pattern[] patterns = (Pattern[])Enum.GetValues(typeof(Pattern));
        Personality[] personalities = (Personality[])Enum.GetValues(typeof(Personality));

        o.w_ = weights[UnityEngine.Random.Range(1, weights.Length)];
        o.hl_ = hairLengths[UnityEngine.Random.Range(1, hairLengths.Length)];
        o.s_ = shines[UnityEngine.Random.Range(1, shines.Length)];
        o.pattern_ = patterns[UnityEngine.Random.Range(1, patterns.Length)];
        o.p_ = personalities[UnityEngine.Random.Range(1, personalities.Length)];
        o.r_ = UnityEngine.Random.Range(0, 256);
        o.g_ = UnityEngine.Random.Range(0, 256);
        o.b_ = UnityEngine.Random.Range(0, 256);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CustomerOrderTrigger"))
        {
            checkpoint = other.gameObject;
        }
    }
}


