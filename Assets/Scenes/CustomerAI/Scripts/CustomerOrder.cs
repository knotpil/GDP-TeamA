using UnityEngine;
using System;
using OrderOwner;

namespace OrderOwner //to make the owner struct accessible in other files
{
    public enum HairLength 
    {
        None = -1,
        //Bald, //commented out since it is a suggestion
        VeryShort = 5,
        Short = 10,
        Medium = 15,
        Long = 20,
        VeryLong = 25
    };

    public enum Weight
    {
        None = -1,
        VerySkinny = 5,
        Skinny = 10,
        Normal = 15,
        Heavy = 20,
        VeryHeavy = 25
    };

    public enum Shine
    {
        None = -1,
        NotShiny = 0,
        LittleShiny = 10,
        Shiny = 20,
        VeryShiny = 30
    };

    public enum Pattern
    {
        Unassigned = -1,
        None = 0,
        Spots = 10,
        Stripes = 20,
        Calico = 30
    };

    public enum Personality
    {
        None = -1,
        VeryTimid = 5,
        Timid = 10,
        Neutral = 15,
        Feisty = 20,
        VeryFeisty = 25
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
        public double cost_ = 0;
        public int tip_;
        public int num_ = 0; //order number
    }
}

public class CustomerOrder : MonoBehaviour
{
    [Header("Checkpoint")]
    public GameObject checkpoint;

    [Header("Placed Order?")]
    public bool placed = false;

    [Header("Received Order?")]
    public bool received = false;

    [Header("Order to Place")]
    public Order o = new Order();

    void Awake()
    {
        o.tip_ = UnityEngine.Random.Range(10, 26); //random tip from $10-$25
    }

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
        o.cost_ = Convert.ToInt32(o.w_) + Convert.ToInt32(o.hl_) + Convert.ToInt32(o.s_) + Convert.ToInt32(o.pattern_);
        o.cost_ += Convert.ToInt32(o.p_) + Convert.ToDouble(o.r_)/15 + Convert.ToDouble(o.g_)/15 + Convert.ToDouble(o.b_)/15;
        o.cost_ = Math.Truncate(o.cost_ * 100) / 100;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("CustomerOrderTrigger"))
        {
            checkpoint = other.gameObject;
        }
    }
}


