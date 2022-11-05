using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Manuscript
{
    [System.Serializable]
    public struct Dialog
    {
        [System.Serializable]
        public struct DialogNode //Each node on the dialog tree is a DialogNode that contains a set amount of lines and usually ends in some kind of prompt
        {
            [System.Serializable]
            public struct Line
            {
                [System.Serializable]
                public class CharacterIdentity
                {
                    public string name;
                    public Sprite mugshot;

                    public CharacterIdentity(string name_in)
                    {
                        name = name_in;
                        mugshot = null;
                    }
                }

                public Line(string line, CharacterIdentity identity)
                {
                    myLine = line;
                    myIdentity = identity;
                }
                public string myLine;
                public CharacterIdentity myIdentity;
            }
            [System.Serializable]
            public struct PromptOption
            {
                public string promptName;
                public DialogNode destinationDialog;

                public PromptOption(string promptName_in, DialogNode destinationDialog_in)
                {
                    promptName = promptName_in;
                    destinationDialog = destinationDialog_in;
                }
            }
            public List<Line> lines;
            public List<PromptOption> options;
            public DialogNode(List<Line> lines_in, List<PromptOption> options_in)
            {
                lines = lines_in;
                options = options_in;
            }
        }
        DialogNode startNode;
        public DialogNode currentNode;
        public Dialog(DialogNode startNode_in)
        {
            startNode = startNode_in;
            currentNode = startNode_in;
        }
    }
}