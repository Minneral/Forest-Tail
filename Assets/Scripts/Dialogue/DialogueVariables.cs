using System.Collections.Generic;
using UnityEngine;
using Ink.Runtime;

public class DialogueVariables
{
    public Dictionary<string, Ink.Runtime.Object> variables { get; private set; }

    private Story globalVariablesStory;
    private const string saveVariablesKey = "INK_VARIABLES";

    public DialogueVariables(TextAsset loadGlobalsJSON)
    {
        // create the story
        globalVariablesStory = new Story(loadGlobalsJSON.text);

        // initialize the dictionary
        variables = new Dictionary<string, Ink.Runtime.Object>();
        foreach (string name in globalVariablesStory.variablesState)
        {
            Ink.Runtime.Object value = globalVariablesStory.variablesState.GetVariableWithName(name);
            variables.Add(name, value);
            Debug.Log("Initialized global dialogue variable: " + name + " = " + value);
        }
    }

    public void SaveVariables()
    {
        if (globalVariablesStory != null)
        {
            // Load the current state of all of our variables to the globals story
            VariablesToStory(globalVariablesStory);
            PlayerPrefs.SetString(saveVariablesKey, globalVariablesStory.state.ToJson());
        }
    }

    public void StartListening(Story story)
    {
        // it's important that VariablesToStory is before assigning the listener!
        VariablesToStory(story);
        story.variablesState.variableChangedEvent += VariableChanged;
    }

    public void StopListening(Story story)
    {
        story.variablesState.variableChangedEvent -= VariableChanged;
    }

    private void VariableChanged(string name, Ink.Runtime.Object value)
    {
        // // only maintain variables that were initialized from the globals ink file
        // if (variables.ContainsKey(name))
        // {
        //     variables.Remove(name);
        //     variables.Add(name, value);
        // }


        // Проверяем, существует ли переменная в словаре
        if (variables.ContainsKey(name))
        {
            variables[name] = value; // Обновляем значение без удаления
            Debug.Log($"Updated variable: {name} = {value}");
        }
        else
        {
            Debug.LogWarning($"Attempted to update variable {name}, but it doesn't exist in the global variables.");
        }
    }

    public void VariablesToStory(Story story)
    {
        var variablesCopy = new Dictionary<string, Ink.Runtime.Object>(variables); // Создаем копию словаря
        foreach (KeyValuePair<string, Ink.Runtime.Object> variable in variablesCopy)
        {
            story.variablesState.SetGlobal(variable.Key, variable.Value);
        }
    }


    /// <summary>
    /// Updates the value of a global variable directly from code.
    /// </summary>
    /// <param name="variableName">The name of the variable to update.</param>
    /// <param name="value">The new value to assign to the variable.</param>
    public void UpdateVariable(string variableName, Ink.Runtime.Object value)
    {
        if (variables.ContainsKey(variableName))
        {
            variables[variableName] = value;

            // Update the value in the global story state as well
            globalVariablesStory.variablesState.SetGlobal(variableName, value);
            Debug.Log("Updated global variable: " + variableName + " = " + value);
        }
        else
        {
            Debug.LogWarning("Variable " + variableName + " does not exist in global variables.");
        }
    }
}
