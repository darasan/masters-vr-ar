using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class WordDictionary : MonoBehaviour
{
    private List<string> dictionary = new List<string>(); //The "word dictionary" is implemented as a list. Just an array of strings (no key-value pairs)

    private void AddWordToDictionary(string word)
    {
        dictionary.Add(word);
        //Debug.Log("Added word to dictionary: " + word);
    }

    private void LoadDictionaryFromFile(string path)
    {
        if (!File.Exists(path))
        {
            Debug.Log("Could not open file at path: " + path);
        }

        else
        {
            Debug.Log("File opened OK: " + path);
        }

        // Open the file and read each line into a string array
        string[] readText = File.ReadAllLines(path);
        foreach (string word in readText)
        {
            if(!dictionary.Contains(word))
            {
                AddWordToDictionary(word);
            }
        }
    }

    public List<string> SearchDictionaryForString(string searchString)
    {
        List<string> searchResults = new List<string>();

        //Remove leading and trailing whitespace
        searchString.Trim();

        //Debug.Log("SearchDictionaryForString: " + searchString);

        string searchPattern = "^" + searchString; //"^" means should match from start of string, else will find results inside string. https://learn.microsoft.com/en-us/dotnet/csharp/how-to/search-strings

        foreach(string word in dictionary)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(word, searchPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                searchResults.Add(word);
            }
        }

        return searchResults;
    }

    private void ShuffleDictionary()  
    {
        //Fisher-Yates shuffle algorithm
        System.Random random = new System.Random();  
        int n = this.dictionary.Count;  

        for(int i= dictionary.Count - 1; i > 1; i--)
        {
            int rnd = random.Next(i + 1);  

            string word = dictionary[rnd];  
            dictionary[rnd] = dictionary[i];  
            dictionary[i] = word;
        }
    }

    private void PrintDictionaryContents()
    {
        Debug.Log("PrintDictionaryContents: ");

        foreach(string word in dictionary)
        {
            Debug.Log(word);
        }

        Debug.Log("Total words in dictionary: " + dictionary.Count);
    }


    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("Application.persistentDataPath: " + Application.persistentDataPath);

        //File stored in app data folder
        string path = Application.persistentDataPath + "/wordList.txt";
        LoadDictionaryFromFile(path);

         //Sort from A-Z (default sort comparer)
        //dictionary.Sort(); 
        ShuffleDictionary();
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
