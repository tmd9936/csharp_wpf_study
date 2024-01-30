using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.PModel;
using FactorySimulation.Utill;
using FactorySimulation.View;
using System.Collections.Generic;

namespace FactorySimulation.Work
{
    public class InWorkThread : WorkThread
    {
        public InWorkThread(ProgressBar _progressBar, TextBlock _box, Color _workOffColor, Color _workOnColor)
            : base(_progressBar, _box,  _workOffColor, _workOnColor)
        {
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Visibility = System.Windows.Visibility.Hidden;
            }));
        }

        protected override void Act()
        {
            if (!LogInit)
            {
                LogManager.Instance.SetLog("제품 투입...");
                LogInit = true;
            }

            Thread.Sleep(2);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                if (progressBar.Maximum > progressBar.Value)
                {
                    progressBar.Value += 1;
                }
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
