using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishDialogue : MonoBehaviour
{
    public Manuscript.Dialog dialog;
    public DialogBox dialogBox;

    public void Start()
    {
        dialogBox.InitiateDialog(dialog);
    }
}
