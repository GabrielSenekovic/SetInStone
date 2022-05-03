using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Linq;

public class InputChange : MonoBehaviour
{
    [SerializeField] InputActionAsset inputAction;
    InputActionMap map;
    string currentScheme = "";

    InputActionRebindingExtensions.RebindingOperation rebindingOperation;

    [SerializeField]Transform buttonListParent;

    List<KeybindButton> buttons = new List<KeybindButton>();
    [SerializeField]Button returnButton;

    private void Start()
    {
        map = inputAction.actionMaps[0];

        buttons = buttonListParent.GetComponentsInChildren<KeybindButton>().ToList();
    }
    public void Initialize(string currentScheme_in)
    {
        currentScheme = currentScheme_in;
        for (int i = 0; i < buttons.Count; i++)
        {
            switch (buttons[i].action)
            {
                case "Jump":
                    buttons[i].SetButtonText(map.actions[2].bindings[map.actions[2].GetBindingIndex(currentScheme)].path);
                    break;
                case "Hookshot":
                    buttons[i].SetButtonText(map.actions[4].bindings[map.actions[4].GetBindingIndex(currentScheme)].path);
                    break;
                case "Attack":
                    buttons[i].SetButtonText(map.actions[7].bindings[map.actions[7].GetBindingIndex(currentScheme)].path);
                    break;
                case "Pulka":
                    buttons[i].SetButtonText(map.actions[5].bindings[map.actions[5].GetBindingIndex(currentScheme)].path);
                    break;
                case "Left":
                    buttons[i].SetButtonText(map.actions[0].bindings[map.actions[0].GetBindingIndex(currentScheme)].path);
                    break;
                case "Right":
                    buttons[i].SetButtonText(map.actions[0].bindings[map.actions[0].GetBindingIndex(currentScheme) + 1].path);
                    break;
                case "Down":
                    buttons[i].SetButtonText(map.actions[8].bindings[map.actions[8].GetBindingIndex(currentScheme)].path);
                    break;
                case "Map":
                    buttons[i].SetButtonText(map.actions[11].bindings[map.actions[11].GetBindingIndex(currentScheme)].path);
                    break;
                case "Pause":
                    buttons[i].SetButtonText(map.actions[10].bindings[map.actions[10].GetBindingIndex(currentScheme)].path);
                    break;
                default:
                    Debug.Break(); break;
            }
        }
    }

    public void Rebind(string function)
    {
        switch(function)
        {
            case "Jump": OnRebind(new List<InputAction>() { map.actions[2], map.actions[3] }, function);
                break;
            case "Hookshot":OnRebind(new List<InputAction>() { map.actions[4]}, function);
                break;
            case "Attack": OnRebind(new List<InputAction>() { map.actions[7] }, function); 
                break;
            case "Pulka": OnRebind(new List<InputAction>() { map.actions[5], map.actions[6] }, function);
                break;
            case "Left": OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "negative");
                break;
            case "Right": OnRebind(new List<InputAction>() { map.actions[0], map.actions[1] }, function, "positive");
                break;
            case "Down": OnRebind(new List<InputAction>() { map.actions[8], map.actions[9] }, function);
                break;
            case "Map": OnRebind(new List<InputAction>() { map.actions[11]}, function);
                break;
            case "Pause": OnRebind(new List<InputAction>() { map.actions[10]}, function);
                break;
            default: Debug.Break(); break;
        }
    }
    public void OnRebind(List<InputAction> actions, string name)
    {
        actions[0].Disable();
        rebindingOperation = actions[0].PerformInteractiveRebinding()
                .WithBindingGroup(currentScheme) //Only bind if has this scheme
                .OnMatchWaitForAnother(0.2f)
                .OnCancel(operation => { Debug.Log("Cancel"); })
                .OnComplete(operation => RebindComplete(actions, name))
                .Start();
    }
    public void OnRebind(List<InputAction> actions, string name, string compositeName)
    {
        Debug.Log("Called");
        actions[0].Disable();
        var bindingIndex = actions[0].bindings.IndexOf(x => x.isPartOfComposite && x.groups.Contains(currentScheme) && x.name == compositeName);
        Debug.Log(bindingIndex);

        rebindingOperation = actions[0].PerformInteractiveRebinding(bindingIndex)
            .WithBindingGroup(currentScheme) //Only bind if has this scheme
            .OnMatchWaitForAnother(0.2f)
            .OnCancel(operation => { Debug.Log("Cancel"); })
            .OnComplete(operation => RebindComplete(actions, name, bindingIndex))
            .Start();
    }
    void RebindComplete(List<InputAction> actions, string name)
    {
        rebindingOperation.Dispose();
        actions[0].Enable();

        string path = actions[0].bindings[actions[0].GetBindingIndex(currentScheme)].overridePath;

        if (actions.Count > 1)
        {
            //Change the associated buttons
            //ie if you change jump, also change cancel jump
            for (int i = 1; i < actions.Count; i++)
            {
                actions[i].ApplyBindingOverride(path);
            }
        }
        Debug.Log("Applying binding");
        ApplyChangesToButtons(path, name);
    }

    void RebindComplete(List<InputAction> actions, string name, int bindingIndex)
    {
        rebindingOperation.Dispose();
        actions[0].Enable();

        string path = actions[0].bindings[bindingIndex].overridePath;

        if (actions.Count > 1)
        {
            //Change the associated buttons
            //Make sure that only one of the two composites get changed, not both
            for (int i = 1; i < actions.Count; i++)
            {
                actions[i].ApplyBindingOverride(bindingIndex, path);
            }
        }
        Debug.Log(map.actions[1].bindings[map.actions[0].GetBindingIndex(currentScheme)].name + map.actions[1].bindings[map.actions[0].GetBindingIndex(currentScheme)].overridePath);
        Debug.Log(map.actions[1].bindings[map.actions[0].GetBindingIndex(currentScheme) + 1].name + map.actions[1].bindings[map.actions[0].GetBindingIndex(currentScheme) + 1].overridePath);

        Debug.Log("Applying binding");
        ApplyChangesToButtons(path, name);
    }

    void ApplyChangesToButtons(string path, string name)
    {
        Game.Instance.keybindConflict = false; //Flag for if a conflict has been detected
        returnButton.gameObject.SetActive(true);
        int indexOfOriginalButton = 0;

        for(int i = 0; i < buttons.Count; i++) //Find the button you changed and apply changes
        {
            if (buttons[i].action == name)
            {
                indexOfOriginalButton = i;
                buttons[i].SetButtonText(path);
            }
        }
        for(int i = 0; i < buttons.Count; i++) //Go through all buttons to find conflicting bindings
        {
            if(i != indexOfOriginalButton && buttons[i].IsSamePath(path))
            {
                buttons[i].SetConflict(true);
                Game.Instance.keybindConflict = true;
                returnButton.gameObject.SetActive(false);
                Debug.Log("Conflict found");
            }
            else
            {
                buttons[i].SetConflict(false);
            }
        }
        if (Game.Instance.keybindConflict)
        {
            buttons[indexOfOriginalButton].SetConflict(true); //If a conflict was still found, set current button into conflict
        }
    }

    bool GetIfContainedInGroup(string input, string currentScheme)
    {
        List<string> groups = new List<string>();
        string currentWord = "";
        for(int i = 0; i < input.Length; i++)
        {
            if(input[i] != ';')
            {
                currentWord += input[i];
            }
            if(input[i] == ';' || i == input.Length - 1)
            {
                groups.Add(currentWord);
                currentWord = "";
            }
        }
        for(int i = 0; i < groups.Count; i++)
        {
            if(groups[i] == currentScheme)
            {
                return true;
            }
        }
        return false;
    }
}