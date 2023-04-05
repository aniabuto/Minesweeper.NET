using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace Games
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool gameOver = false;
        private bool win = false;
        private int width = 8;
        private int height = 8;
        private int bombs = 10;
        private int discovered = 0;
        private int flags = 0;
        private int time = 0;
        private int fieldSize = 30;
        private Field[,] fields;
        private int firstRow;
        private int firstCol;
        private bool isPlaying = false;
        private DispatcherTimer timer;

        public MainWindow()
        {
            InitializeComponent();
            StartGame();
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += UpdateLabel;
        }

        private void UpdateLabel(object? sender, EventArgs e)
        {
            time++;
            Timer.Content = time;
        }

        public void SetCustomLevel(int width, int height, int bombs)
        {
            this.width = width;
            this.height = height;
            this.bombs = bombs;
            flags = 0;
            time = 0;

            StartGame();
        }
        
        private void CustomLevel_OnClick(object sender, RoutedEventArgs e)
        {
            //Custom.IsOpen = true;
            CustomDifficulty popup = new CustomDifficulty();
            popup.Owner = this;
            popup.setcustomlevel += SetCustomLevel;
            popup.ShowDialog();
        }


        private void Beginner_OnClick(object sender, RoutedEventArgs e)
        {
            width = 8;
            height = 8;
            bombs = 10;
            flags = 0;
            time = 0;

            StartGame();
        }
        
        private void Intermediate_OnClick(object sender, RoutedEventArgs e)
        {
            width = 16;
            height = 16;
            bombs = 40;
            flags = 0;
            time = 0;

            StartGame();
        }

        private void Expert_OnClick(object sender, RoutedEventArgs e)
        {
            width = 31;
            height = 16;
            bombs = 99;
            flags = 0;
            time = 0;

            StartGame();
        }

        private void StartGame()
        {
            gameOver = false;
            win = false;
            flags = 0;
            time = 0;
            isPlaying = false;
            discovered = 0;
            RestartButton.Content = "🙂";
            BombCounter.Content = bombs - flags;
                
            mineField.Children.Clear();
            mineField.ColumnDefinitions.Clear();
            mineField.RowDefinitions.Clear();
            
            fields = new Field[width, height];
            int w, h;
            for (w = 0; w < width; w++)
            {
                mineField.ColumnDefinitions.Add(new ColumnDefinition());
            }
            for (h = 0; h < height; h++)
            {
                mineField.RowDefinitions.Add(new RowDefinition());
            }

            this.Width = fieldSize * w + 60;
            this.Height = fieldSize * h + 200;

            Timer.Margin = new Thickness(0, 0, width / 2 - 165, 0);
            BombCounter.Margin = new Thickness(width / 2 - 165, 0, 0, 0);
            RestartButton.Margin = new Thickness(width / 2 - 23, 0, width / 2 - 23, 0);

            mineField.Width = fieldSize * w;
            mineField.Height = fieldSize * h;

            for (w = 0; w < width; w++)
            {
                for (h = 0; h < height; h++)
                {
                    fields[w, h] = new Field(h, w);
                    fields[w, h].Background = Brushes.Green;
                    //fields[w, h].Click += DiscoverEvent;
                    //fields[w, h].MouseRightButtonDown += Suspense;
                    fields[w, h].MouseRightButtonUp += FlagEvent;
                    fields[w, h].PreviewMouseLeftButtonDown += Suspense;
                    fields[w, h].PreviewMouseLeftButtonUp += PlaceFirst;
                    fields[w, h].PreviewMouseLeftButtonUp += ReleaseMouse;
                    //fields[w, h].MouseRightButtonUp += ReleaseMouse;
                    Grid.SetColumn(fields[w, h], w);
                    Grid.SetRow(fields[w, h], h);
                    mineField.Children.Add(fields[w, h]);
                }
            }


        }

        private void PlaceFirst(object sender, MouseButtonEventArgs e)
        {
            firstRow = ((Field) sender).GetRow();
            firstCol = ((Field) sender).GetCol();

            int bombsToPlace = bombs;
            while (bombsToPlace > 0)
            {
                Random random = new Random();
                int row = random.Next(height);
                int col = random.Next(width);
                if (row != firstRow && col != firstCol && !fields[col, row].IsMine())
                {
                    bool isNeighbour = false;
                    int i, j;
                    for (i = -1; i <= 1; i++)
                    {
                        int neww = firstCol + i;
                        if (neww >= 0 && neww < width)
                        {
                            for (j = -1; j <= 1; j++)
                            {
                                int newh = firstRow + j;
                                if ( newh >= 0 && newh < height)
                                {
                                    if (neww == col && newh == row)
                                        isNeighbour = true;
                                }
                            }
                        }
                    }
                    if (!isNeighbour)
                    {
                        bombsToPlace--;
                        fields[col, row].PlaceMine();   
                    }
                }
                
                timer.Start();
                
            }
            
            PlaceNeighbours();
            
            Discover(firstRow, firstCol);
            
            int w, h;
            for (w = 0; w < width; w++)
            {
                for (h = 0; h < height; h++)
                {
                    fields[w, h].PreviewMouseLeftButtonUp -= PlaceFirst;
                    fields[w, h].PreviewMouseLeftButtonUp += DiscoverEvent;
                }
            }

            isPlaying = true;
        }


        private void PlaceNeighbours()
        {
            int w, h;
            for (w = 0; w < width; w++)
            {
                for (h = 0; h < height; h++)
                {
                    int i, j;
                    for (i = -1; i <= 1; i++)
                    {
                        int neww = w + i;
                        if (neww >= 0 && neww < width)
                        {
                            for (j = -1; j <= 1; j++)
                            {
                                int newh = h + j;
                                if ( newh >= 0 && newh < height)
                                {
                                    if (fields[neww, newh].IsMine())
                                        fields[w, h].AddMineNeighbour();
                                }
                            }
                        }
                    }
                }
            }
        }
        
        private void FlagEvent(object sender, MouseButtonEventArgs e)
        {
            if (!gameOver && !win)
            {
                Flag(((Field) sender).GetRow(), ((Field) sender).GetCol());
            }
        }
        
        private void Suspense(object sender, MouseButtonEventArgs e)
        {
            if (!gameOver && !win && !((Field) sender).IsDiscovered())
            {
                RestartButton.Content = "😮";
            }
        }

        private void ReleaseMouse(object sender, MouseButtonEventArgs e)
        {
            if (!gameOver && !win)
            {
                RestartButton.Content = "🙂";
            }
        }
        
        private void DiscoverEvent(object sender, MouseButtonEventArgs e)
        {
            if(!gameOver && !win)
                Discover(((Field) sender).GetRow(), ((Field) sender).GetCol());
        }

        private void Flag(int row, int col)
        {
            if (!fields[col, row].IsDiscovered())
            {
                fields[col, row].ToggleFlag();
                if (fields[col, row].IsFlagged())
                {
                    flags++;
                    fields[col, row].Foreground = Brushes.Red;
                    fields[col, row].Content = "🚩";
                }
                else
                {
                    flags--;
                    fields[col, row].Foreground = Brushes.Black;
                    fields[col, row].Content = " ";
                }
                    
                BombCounter.Content = bombs - flags;
            }
        }
        
        private void Discover(int row, int col)
        {
            // returns true if bomb found

            Field field = fields[col, row];

            if (field.IsMine() && !field.IsFlagged())
            {
                field.Content = " ";
                field.Background = Brushes.Black;
                field.Foreground = Brushes.Black;
                gameOver = true;
                RestartButton.Content = "😵";
                timer.Stop();
                return;
            }
            if (!field.IsDiscovered() && !field.IsFlagged())
            {
                discovered++;
                field.Discover();
                field.Background = Brushes.DodgerBlue;
                // discover "island" of blank fields
                if (field.GetMinesInNeighbourHood() == 0)
                {
                    field.Foreground = Brushes.DodgerBlue;
                    field.Content = " ";
                    int i, j;
                    for (i = -1; i <= 1; i++)
                    {
                        int newcol = col + i;
                        if (newcol >= 0 && newcol < width)
                        {
                            for (j = -1; j <= 1; j++)
                            {
                                int newrow = row + j;
                                if (newrow >= 0 && newrow < height)
                                {
                                    Discover(newrow, newcol);
                                }
                            }
                        }
                    }
                }
            }

            if (discovered + bombs == width * height)
                win = true;
            if (win)
            {
                RestartButton.Content = "😎";
                timer.Stop();
            }
                
        }


        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            StartGame();
        }
    }
}