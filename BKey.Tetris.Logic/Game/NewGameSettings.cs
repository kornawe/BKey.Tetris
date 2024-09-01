using BKey.Tetris.Logic.Board;
using System;
using System.ComponentModel.DataAnnotations;

namespace BKey.Tetris.Logic.Game;
public class NewGameSettings
{
    public int Seed { get; set; } = Guid.NewGuid().GetHashCode();

    [Required]
    public BoardCreateOptions BoardCreateOptions { get; set; } = new BoardCreateOptions();

}
