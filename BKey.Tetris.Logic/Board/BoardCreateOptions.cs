using System;
using System.ComponentModel.DataAnnotations;

namespace BKey.Tetris.Logic.Board;
public class BoardCreateOptions
{
    [Range(10, 20)]
    public int Width { get; set; } = 10;

    [Range(10, 40)]
    public int Height { get; set; } = 20;
}
