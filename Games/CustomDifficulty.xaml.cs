using System;
using System.Windows;
using System.Windows.Controls;

namespace Games
{
    public partial class CustomDifficulty : Window
    {
        public CustomDifficulty()
        {
            InitializeComponent();
        }

        private void Upper_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(width.Text);
            val++;
            if (val >= 35)
                val = 35;
            width.Text = val.ToString();
        }

        private void Lower_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(width.Text);
            val--;
            if (val <= 8)
                val = 8;
            width.Text = val.ToString();
        }

        private void UpperH_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(height.Text);
            val++;
            if (val >= 35)
                val = 35;
            height.Text = val.ToString();
        }

        private void LowerH_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(height.Text);
            val--;
            if (val <= 8)
                val = 8;
            height.Text = val.ToString();
        }

        private void UpperB_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(bombs.Text);
            val++;
            int mineFieldSize = Int32.Parse(height.Text) * Int32.Parse(height.Text);
            if (val > mineFieldSize / 3)
                val = mineFieldSize / 3;
            bombs.Text = val.ToString();
        }

        private void LowerB_OnClick(object sender, RoutedEventArgs e)
        {
            int val = Int32.Parse(bombs.Text);
            val--;
            if (val <= 1)
                val = 1;
            bombs.Text = val.ToString();
        }

        private void Save_OnClick(object sender, RoutedEventArgs e)
        {
            setcustomlevel(Int32.Parse(width.Text), Int32.Parse(height.Text), Int32.Parse(bombs.Text));
            this.Close();
        }

        public event Action<int, int, int> setcustomlevel;

        private void Bombs_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                int val = Int32.Parse(bombs.Text);
                if (val <= 1)
                    val = 1;
                int mineFieldSize = Int32.Parse(height.Text) * Int32.Parse(height.Text);
                if (val > mineFieldSize / 3)
                    val = mineFieldSize / 3;
                bombs.Text = val.ToString();
            }
            catch (Exception exception)
            {
                bombs.Text = "1";
            }
        }

        private void Width_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                int val = Int32.Parse(width.Text);
                if (val <= 8)
                    val = 8;
                if (val >= 35)
                    val = 35;
                width.Text = val.ToString();
            }
            catch (Exception exception)
            {
                width.Text = "8";
            }
        }

        private void Height_OnTextChanged(object sender, TextChangedEventArgs e)
        {
            try
            {
                
                int val = Int32.Parse(height.Text);
                if (val <= 8)
                    val = 8;
                if (val >= 35)
                    val = 35;
                height.Text = val.ToString();
            }
            catch (Exception exception)
            {
                height.Text = "8";
            }
        }
    }
}

