﻿using System.Windows.Forms;

namespace ZorkGUI.Forms
{
    public partial class AddRoomForm : Form
    {
        public string RoomName
        {
            get => roomNameTextBox.Text;
            set => roomNameTextBox.Text = value;
        }

        public AddRoomForm()
        {
            InitializeComponent();
        }

        private void roomNameTextBox_TextChanged(object sender, System.EventArgs e)
        {
            okButton.Enabled = !string.IsNullOrEmpty(RoomName);
        }

        private void roomNameTextBox_Enter(object sender, System.EventArgs e)
        {
            okButton.Enabled = !string.IsNullOrEmpty(RoomName);
        }
    }
}
