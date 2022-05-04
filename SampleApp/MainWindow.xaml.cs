using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window,INotifyPropertyChanged
    {
        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            Task.Run(async () =>
            {
                while (true)
                {
                    var position = AutoClicker.Actions.Mouse.GetPos();
                    Footer = $"{position.X} {position.Y}";
                    await Task.Delay(100);
                }
            });
        }

        protected void NotifyPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            if (PropertyChanged != null) PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        private string? script = 
@"SetImageDir image
Wait 1000
SetMousePos 0 0
Wait 1000
MoveToImage template.png Center
Wait 1000
Click
";

        public string? Script
        {
            get { return script; }
            set { script = value; NotifyPropertyChanged(); }
        }

        private string footer = "";
        public string Footer
        {
            get { return footer; }
            set { footer = value; NotifyPropertyChanged(); }
        }

        public class RunCommand : RunCommandBase, ICommand
        {
            private bool canExecute = true;

            public bool CanExecute(object? parameter)
            {
                return canExecute;
            }

            public async void Execute(object? parameter)
            {
                if (parameter == null)
                {
                    return;
                }
                canExecute = false;
                var engine = new AutoClicker.Engine.BaseEngine();
                await engine.ExecuteScriptAsync((string)parameter);
                canExecute = true;
            }
        }

        public RunCommand Run { get; private set; } = new RunCommand();
    }
}
