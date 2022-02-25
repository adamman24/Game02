using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;

namespace Game02.Screens
{
    //event argument includes the index of player who triggered event
    public class PlayerIndexEventArgs
    {
        public PlayerIndex PlayerIndex { get; }

        public PlayerIndexEventArgs(PlayerIndex playerIndex)
        {
            PlayerIndex = playerIndex;
        }
    }
}
