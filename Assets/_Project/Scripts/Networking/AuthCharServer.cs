/**
 * AuthCharServer.cs
 * Created by: Joao Borks
 * Created on: 30/04/17 (dd/mm/yy)
 */

using UnityEngine;
using System.Collections.Generic;

public class AuthCharServer : MonoBehaviour
{
    Queue<Vector2> inputBuffer;
    AuthoritativeCharacter character;
    int movesMade;
    int serverTick;

    CharacterController charCtrl;

    void Awake()
    {
        inputBuffer = new Queue<Vector2>();
        character = GetComponent<AuthoritativeCharacter>();
        character.state = CharacterState.Zero;
        charCtrl = GetComponent<CharacterController>();
    }

    void Update()
    {
        if (movesMade > 0)
            movesMade--;
        if (movesMade == 0)
        {
            CharacterState state = character.state;
            while (movesMade < character.InputBufferSize && inputBuffer.Count > 0)
            {
                state = CharacterState.Move(state, inputBuffer.Dequeue(), character.Speed, serverTick);
                charCtrl.Move(state.position - charCtrl.transform.position);
                movesMade++;
            }
            if (movesMade > 0)
            {
                state.position = transform.position;
                character.state = state;
                character.OnServerStateChange(state);
            }
        }
    }

    void FixedUpdate()
    {
        serverTick++;        
    }

    public void Move(Vector2[] inputs)
    {
        foreach (var input in inputs)
            inputBuffer.Enqueue(input);
    }
}