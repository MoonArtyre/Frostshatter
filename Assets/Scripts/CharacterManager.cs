using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
public class CharacterManager : MonoSingleton<CharacterManager>
{
    public Dictionary<CharacterObject, string> characters;
    public List<CharacterObject> InitChars;

    private void Start()
    {
        characters = new Dictionary<CharacterObject, string>();

        for (int i = 0; i < InitChars.Count; i++)
        {
            characters.Add(InitChars[i], InitChars[i].characterName);
        }
    }

    public string GetCharacterName(CharacterObject _char)
    {
        return characters[_char];
    }

    public void SetCharacterName(CharacterObject _char, string newName)
    {
        characters[_char] = newName;
    }
}


