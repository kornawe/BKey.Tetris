using System;
using System.Threading.Tasks;
using BKey.Tetris.Console.Input;

namespace BKey.Tetris.Console.Menu;

public class MenuItemBack : MenuItemAction
{


    public MenuItemBack(MenuController menuController)
        : base("Back", menuController.Back)
    {
    }


}