using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomChatter : MonoBehaviour
{
    public List<DialogScriptobject> chatterList;
    public DialogScriptobject exhaustedChatter;

    public void PlayChatter()
    {
        DialogScriptobject newPrompt = exhaustedChatter;

        if(chatterList.Count > 0)
        {
            newPrompt = chatterList[Random.Range(0, chatterList.Count)];
            chatterList.Remove(newPrompt);
        }

        DialogUIManager.Instance.DialogStart(newPrompt);
    }
}
