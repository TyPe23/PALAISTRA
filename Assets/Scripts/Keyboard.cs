using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using HighScore;

public class Keyboard : MonoBehaviour
{
    
    public string word = "";
    public TMP_Text output = null;
    private bool shift = false;
    private EndOfGame EOG;

    public IDictionary<string, string> shiftDictionary = new Dictionary<string, string>() {
        {"a", "A"}, {"b", "B"}, {"c", "C"}, {"d", "D"},
        {"e", "E"}, {"f", "F"}, {"g", "G"}, {"h", "H"},
        {"i", "I"}, {"j", "J"}, {"k", "K"}, {"l", "L"},
        {"m", "M"}, {"n", "N"}, {"o", "O"}, {"p", "P"},
        {"q", "Q"}, {"r", "R"}, {"s", "S"}, {"t", "T"},
        {"u", "U"}, {"v", "V"}, {"w", "W"}, {"x", "X"},
        {"y", "Y"}, {"z", "Z"},
    };


    public void typingFunct(string letter)
    {
        //check if the character is not in the dictionary
        //or shift is false
        if (shiftDictionary.ContainsKey(letter) & shift == true)
        {
            //if so, set to capital version of the letter
            letter = shiftDictionary[letter];
        }

        word = word + letter;

        printFunct(word); // send to text mesh pro
        shift = false;
    }

    public void backspaceFunct()
    {
        if (word != "")
        {
            word = word.Substring(0, word.Length - 1);
        }
        printFunct(word);
    }

    public void shiftFunct()
    {
        shift = true;
    }


    public void printFunct(string input)
    {
        output.text = input;
    }

    public void enterFunct()
    {
        EOG = GameObject.FindGameObjectWithTag("EOG").GetComponent<EndOfGame>();
        EOG.enterFunct(word);
    }
}
