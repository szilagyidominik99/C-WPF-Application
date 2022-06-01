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
    /// Interaction logic for MultiPlayer1.xaml
    /// </summary>
    public partial class MultiPlayer1 : UserControl
    {

        public event EventHandler player1;

        public MultiPlayer1()
        {
            InitializeComponent();

        }

        private void Player1(object sender, RoutedEventArgs e)
        {
            player1(this, e);
        }
    }
}
