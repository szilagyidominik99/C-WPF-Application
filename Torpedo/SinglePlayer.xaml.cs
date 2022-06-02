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
    /// Interaction logic for SinglePlayer.xaml
    /// </summary>
    public partial class SinglePlayer : UserControl
    {

        public event EventHandler singlePlayer;
        public SinglePlayer()
        {
            InitializeComponent();
        }

        public void Player(object sender, RoutedEventArgs e)
        {
            singlePlayer(this, e);
        }
    }
}
