using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Threading;


namespace FactorySimulation.Utill
{
    public class LogManager
    {
        public static LogManager instance;

        private TextBlock logTextBox = null;

        private LogManager()
        {

        }

        public static LogManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new LogManager();
                }
                return instance;
            }
        }

        public void Initialize(TextBlock textBox)
        {
            if (IsInit)
            {
                return;
            }

            if (textBox == null)
            {
                return;
            }

            logTextBox = textBox;

            IsInit = true;
        }

        public void SetLog(string text)
        {
            if (!IsInit)
            {
                return;
            }

            _ = logTextBox.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                logTextBox.Text = text;
            }));
        }

        private bool IsInit { get; set; }
    }
}
