using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using HurtowniaNapojow.Helpers;
using MessageBox = System.Windows.MessageBox;

namespace HurtowniaNapojow.Utils
{
    public partial class Prompt
    {
        public Prompt()
        {
            InitializeComponent();
            PromptMessageTextBlock.Focus();
        }

        public string ResponseText
        {
            get { return PromptResponseTextBox.Text; }
            set { PromptResponseTextBox.Text = value; }
        }

        public string PromptMessage
        {
            set { PromptMessageTextBlock.Text = value; }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrEmpty(PromptResponseTextBox.Text))
            {
                MessageBox.Show("Podaj wartość!", Globals.TITLE_ERROR);
            }
            else
            {
                DialogResult = true;
            }
        }
        private void PromptResponseTextBox_OnKeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                ConfirmButton.PerformClick();
            }
        }
    }
}