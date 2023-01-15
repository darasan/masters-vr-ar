using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WordDictionary : MonoBehaviour
{
    public string[] wordsA = 
    {"Any", "Arm", "Allow", "Apple", "Author", "Apricot", "Art", "Audio"};

    public string[] wordsB = 
    {"Back", "Be", "Best", "Bill", "Bore", "Bubble", "Beach", "Bump"};

    public string[] wordsC = 
    {"Call", "Catch", "Certain", "Check", "Chart", "Chill", "Chew", "Court"};


    private List<string> dictionary = new List<string>(); //The "word dictionary" is implemented as a list. Just an array of strings (no key-value pairs)

    private void AddWordToDictionary(string word)
    {
        dictionary.Add(word);
        //Debug.Log("Added word to dictionary: " + word);
    }

    public List<string> SearchDictionaryForString(string searchString)
    {
        List<string> searchResults = new List<string>();

        //Remove leading and trailing whitespace
        searchString.Trim();

        Debug.Log("SearchDictionaryForString: " + searchString);

        string searchPattern = "^" + searchString; //"^" means should match from start of string, else will find results inside string. https://learn.microsoft.com/en-us/dotnet/csharp/how-to/search-strings

        foreach(string word in dictionary)
        {
            if (System.Text.RegularExpressions.Regex.IsMatch(word, searchPattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                searchResults.Add(word);
            }
        }

        /*foreach(string result in searchResults)
        {
            Debug.Log("Match found: " + result);
        }*/

        return searchResults;
    }

    private void PrintDictionaryContents()
    {
        Debug.Log("PrintDictionaryContents");

        foreach(string word in dictionary)
        {
            Debug.Log(word);
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        foreach(string word in wordsA)
        {
            AddWordToDictionary(word);
        }

        foreach(string word in wordsB)
        {
            AddWordToDictionary(word);
        }

        foreach(string word in wordsC)
        {
            AddWordToDictionary(word);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
