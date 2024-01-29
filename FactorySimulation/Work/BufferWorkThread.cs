using System;
using System.Windows.Controls;
using System.Windows.Media;
using System.Threading;
using System.Windows.Threading;
using FactorySimulation.PModel;
using FactorySimulation.Utill;
using FactorySimulation.View;


namespace FactorySimulation.Work
{
    public class BufferWorkThread : WorkThread
    {
        public BufferWorkThread(ProgressBar _progressBar, TextBlock _box,  Color _workOffColor, Color _workOnColor)
            : base(_progressBar, _box, _workOffColor, _workOnColor)
        {
        }

        protected override void Act()
        {
            if (!LogInit)
            {
                LogManager.Instance.SetLog("제품 대기");
                LogInit = true;
            }

            Thread.Sleep(4);

            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                
                if (progressBar.Maximum > progressBar.Value)
                    progressBar.Value += 1;
                else
                {
                    lock (IsComplete)
                    {
                        IsComplete = true;
                    }
                }
            }));
        }

    }
}
