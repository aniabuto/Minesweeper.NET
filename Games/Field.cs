using System.Windows.Controls;

namespace Games
{
    public class Field : Button
    {
        private bool mine = false;
        private bool flagged = false;
        private bool discovered = false;
        private int minesInNeighburhood = 0;
        private int row;
        private int col;
    
        public Field(int row, int col)
        {
            this.row = row;
            this.col = col;
            this.Content = " ";
        }
    
        public int GetRow()
        {
            return row;
        }
    
        public int GetCol()
        {
            return col;
        }
        
        public void PlaceMine()
        {
            mine = true;
        }
    
        public void ToggleFlag()
        {
            flagged = !flagged;
            if (flagged)
                this.Content = "F";
            else
            {
                this.Content = " ";
            }
        }
    
        public void Discover()
        {
            discovered = true;
            this.Content = minesInNeighburhood.ToString();
        }
    
        public void AddMineNeighbour()
        {
            minesInNeighburhood++;
        }
    
        public bool IsMine()
        {
            return mine;
        }
    
        public bool IsFlagged()
        {
            return flagged;
        }
    
        public bool IsDiscovered()
        {
            return discovered;
        }
    
        public int GetMinesInNeighbourHood()
        {
            return minesInNeighburhood;
        }
    
    }
}


