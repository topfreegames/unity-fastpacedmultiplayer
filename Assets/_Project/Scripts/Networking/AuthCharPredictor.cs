/**
 * AuthCharPredictor.cs
 * Created by: Joao Borks
 * Created on: 30/04/17 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections.Generic;

public class AuthCharPredictor : MonoBehaviour, IAuthCharStateHandler
{
    List<CharacterState> states;
    AuthoritativeCharacter character;
    CharacterState predictedState;
    private int lastProcessedInput;

    void Awake()
    {
        states = new List<CharacterState>();
        character = GetComponent<AuthoritativeCharacter>();
        predictedState = character.state;
    }

    public void AddInput(Vector2 input)
    {
        predictedState = CharacterState.Move(predictedState, input, character.Speed, 0);
        states.Add(predictedState);
        character.SyncState(predictedState);
    }

    public void OnStateChange(CharacterState newState)
    {
        if (states.Count > 0)
        {
            while (
                states.Count > 0 &&
                states[0].moveNum <= newState.moveNum)
            {
                states.RemoveAt(0);
            }
        }
        predictedState = newState;           
        UpdatePredictedState();
    }

    void UpdatePredictedState()
    {
        foreach (CharacterState state in states)
        {
            predictedState = CharacterState.Move(predictedState, state.input, character.Speed, 0);
        }
        character.SyncState(predictedState);
    }
}