﻿using System;
using System.Windows;
using System.Windows.Controls;

namespace Bread_Tools.Resources.Pages
{
    public partial class CommandPage : Page
    { 
        public CommandPage()
        {
            InitializeComponent();

            this.OpenWSLHere.Loaded += this.LoadField;
            this.OpenCommandPromptHere.Loaded += this.LoadField;
            this.OpenPowerShellHere.Loaded += this.LoadField;

            this.Position.Loaded += this.LoadField;

            ////////////////////

            this.OpenWSLHere.Switched += this.SaveField;
            this.OpenCommandPromptHere.Switched += this.SaveField;
            this.OpenPowerShellHere.Switched += this.SaveField;

            this.Position.SelectionChanged += this.SaveField;

            Settings.Data.command.OpenAdminCommandPromptHere = this.OpenCommandPromptHere.IsOn;
            Settings.Data.command.OpenAdminPowerShellHere = this.OpenPowerShellHere.IsOn;
        }

        private void OpenCommandPromptHere_Click(object sender, RoutedEventArgs e)
            => Settings.Data.command.OpenAdminCommandPromptHere = this.OpenCommandPromptHere.IsOn;

        private void OpenPowerShellHere_Click(object sender, RoutedEventArgs e)
            => Settings.Data.command.OpenAdminPowerShellHere = this.OpenPowerShellHere.IsOn;

        private void SaveField(object sender, EventArgs e)
            => Settings.SaveUISettings<Types.CommandTools.Settings>(sender, Settings.Data.command);

        private void LoadField(object sender, EventArgs e)
            => Settings.LoadUISettings<Types.CommandTools.Settings>(sender, Settings.Data.command);

        private void SaveButton_Click(object sender, RoutedEventArgs e)
            => Settings.SaveSettings();
    }
}