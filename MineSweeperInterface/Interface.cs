using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace MineSweeperInterface
{
    public enum GAME_EVENT
    {
        COLLECT_OBJECT, GAMEOVER,
    }

    public class GameboardEventArgs : EventArgs
    {
        private GAME_EVENT gameboardEvent;
        public GAME_EVENT GameboardEvent { get { return gameboardEvent; } }
        public GameboardEventArgs(GAME_EVENT e)
        {
            gameboardEvent = e;
        }
    }
    public interface IMineSweeperGame
    { 
        void Initialize(int row, int column, int mine, System.Windows.Window window);
    }
}
