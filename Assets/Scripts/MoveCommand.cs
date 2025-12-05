using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCommand : ICommand
{
    CommandType ICommand.commandType { get; set; } = CommandType.Move;

    void ICommand.Execute(Player player)
    {
        Debug.Log("∑¢ÀÕMove÷∏¡Ó");
        player.Move();
    }
}
