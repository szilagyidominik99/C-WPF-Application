﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Torpedo
{
    /// <summary>
    /// Interaction logic for SinglePlayerName.xaml
    /// </summary>
    public partial class SinglePlayerName : UserControl
    {
        public event EventHandler startSP;
        public SinglePlayerName()
        {
            InitializeComponent();
        }

        private void OnStartGame(object sender, RoutedEventArgs e)
        {
            if(playerName.Text.Equals(""))
            {
                MessageBox.Show("Give your name!");
            }
            else
            {
                startSP(this, e);
            }
        }
    }
}
