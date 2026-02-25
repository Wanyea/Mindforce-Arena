using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Handles the mana for the player as well as visually updating the mana bar UI element
/// </summary>
public class ManaBar : MonoBehaviour
{
    private Mana mana;
    public Image barImage;
    public TMP_Text manaText;

    public Mana ManaInstance => mana;

    private void Awake()
    {
        mana = new Mana();
    }

    private void Update()
    {
        mana.Update();
        barImage.fillAmount = mana.GetManaNormalized();
        UpdateManaText();
    }

    /// <summary>
    /// Updates the text on the mana bar with the current amount of mana
    /// </summary>
    private void UpdateManaText()
    {
        manaText.text = Mathf.RoundToInt(mana.GetManaAmount()).ToString();
    }

    /// <summary>
    /// Calls and returns the Mana class function HasEnoughMana
    /// </summary>
    /// <param name="amount">
    /// refers to the amount of mana being checked
    /// </param>
    /// <returns>
    /// returns true or false depending if there is enough mana
    /// </returns>
    public bool HasEnoughMana(int amount)
    {
        return mana.HasEnoughMana(amount);
    }

    /// <summary>
    /// calls and returns the Mana class function TryUseMana
    /// </summary>
    /// <param name="amount">
    /// refers to the amount of mana being used
    /// </param>
    public void UseMana(int amount)
    {
        mana.TryUseMana(amount);
    }
}

/// <summary>
/// Handles the mana for the player as well as visually updating the mana bar UI element
/// </summary>
public class Mana
{
    public const int MANA_MAX = 100;

    private float manaAmount;
    private float manaRegenAmount;

    /// <summary>
    /// Defines start mana and mana regenaration rate
    /// </summary>
    public Mana()
    {
        manaAmount = 0;
        manaRegenAmount = 10f;
    }

    public void Update()
    {
        manaAmount += manaRegenAmount * Time.deltaTime;
        manaAmount = Mathf.Clamp(manaAmount, 0f, MANA_MAX);
    }

    /// <summary>
    /// if the current amount of mana is greater than the amount trying to be used,
    /// then the mana will be used and subtracted from the current mana
    /// </summary>
    /// <param name="amount">
    /// refers to the amount of mana trying to be used
    /// </param>
    public void TryUseMana(int amount)
    {
        if(manaAmount >= amount)
        {
            manaAmount -= amount;
        }
    }

    /// <summary>
    /// Gets the current amount of mana
    /// </summary>
    /// <returns>
    /// returns the current amount of mana
    /// </returns>
    public float GetManaAmount()
    {
        return manaAmount;
    }

    /// <summary>
    /// gets a normalized amount of mana
    /// </summary>
    /// <returns>
    /// returns normalized mana amount
    /// </returns>
    public float GetManaNormalized()
    {
        return manaAmount / MANA_MAX;
    }

    /// <summary>
    /// checks if the current amount of mana is greater than or equal to the amount trying to be used
    /// </summary>
    /// <param name="amount">
    /// refers to the amount of mana being checked
    /// </param>
    /// <returns>
    /// true if the current amount of mana is greater than or equal to the amount of mana trying to be used 
    /// </returns>
    public bool HasEnoughMana(int amount)
    {
        return manaAmount >= amount;
    }
}
