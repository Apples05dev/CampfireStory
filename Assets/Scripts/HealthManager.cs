using UnityEngine;

public class HealthManager : MonoBehaviour
{
    [SerializeField] private string _animName_Full = "Full";
    [SerializeField] private string _animName_Half = "Half";
    [SerializeField] private string _animName_Empty = "Empty";

    [Space(10)]
    [SerializeField] private AnimatedObject[] _heartObjects;
    public int Health { get; private set; }
    public int MaxHealth { get; private set; }

    private void Start()
    {
        ResetHealth();
    }

    private void UpdateHearts()
    {
        for (int i = _heartObjects.Length - 1; i >= 0; i--)
        {
            // If health is higher or equal to i*2, the heart is full.
            // If health is lower than i*2 but equal if incremented by 1, the heart is half-full.
            // If health is still lower than i*2 then, the heart is empty.
            if (Health < (i + 1) * 2)
            {
                if (Health + 1 == (i + 1) * 2)
                {
                    _heartObjects[i].SetAnimation(_animName_Half);
                    continue;
                }
                
                _heartObjects[i].SetAnimation(_animName_Empty);
            }
            else
            {
                _heartObjects[i].SetAnimation(_animName_Full);
            }
        }
    }

    /// <summary>
    /// Decrement the object's health value. Returns false if the object's health has reached zero.
    /// </summary>
    /// <param name="amount">Amount of damage to deal.</param>
    /// <returns></returns>
    public bool Damage(int amount)
    {
        Health -= amount;
        UpdateHearts();

        return Health > 0;
    }

    public void ResetHealth()
    {
        Health = _heartObjects.Length * 2;
        UpdateHearts();
    }

    //  TODO: MAKE BEAR FASTER WHEN AT HALF HEALTH???
}
