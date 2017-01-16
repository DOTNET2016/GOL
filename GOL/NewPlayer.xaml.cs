using System;
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
using System.Windows.Shapes;

namespace GOL
{
    /// <summary>
    /// Interaction logic for NewPlayer.xaml
    /// </summary>
    public partial class NewPlayer : Window
    {
        public NewPlayer(string question, string defaultAnswer = "")
        {
            InitializeComponent();
            //sets some default values given when the dialog was opened
            PlayerName.Content = question;
            NameInput.Text = defaultAnswer;
        }

        //return the userInput to PickPlayerWin
        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            NameInput.SelectAll();
            NameInput.Focus();
        }

        //returns the name the user inserted in the textInput
        public string InsertedPlayerName
        {
            get { return NameInput.Text; }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }
    }
}
