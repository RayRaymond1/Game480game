using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class WordBankBoss : MonoBehaviour
{
    public List<string> originalWords = new List<string> 
    { 
        "Honor", "Strength", "Skill", "Clarity", "Loyalty", "Mastery", "Bushido"
    };
    private List<string> currentWords = new List<string>();

    void Awake()
    {
        currentWords.AddRange(originalWords);
        Shuffle(currentWords);
        ConvertToLower(currentWords);
    }

    void Shuffle(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            string temp = words[i];
            int randomIndex = Random.Range(i, words.Count);
            words[i] = words[randomIndex];
            words[randomIndex] = temp;
        }
    }

    void ConvertToLower(List<string> words)
    {
        for (int i = 0; i < words.Count; i++)
        {
            words[i] = words[i].ToLower();
        }
    }

    public string GetWord()
    {
        string newWord = string.Empty;
        if(currentWords.Count != 0)
        {
            newWord = currentWords.Last();
            currentWords.Remove(newWord);
        }
        return newWord;
    }
}
