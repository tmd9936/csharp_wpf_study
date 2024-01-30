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
    public class ClassifyWorkThread : WorkThread
    {
        public ClassifyWorkThread(ProgressBar _progressBar, TextBlock _box, Color _workOffColor, Color _workOnColor)
            : base(_progressBar, _box, _workOffColor, _workOnColor)
        {

        }

        protected override void Act()
        {
            if (!LogInit)
            {
                LogManager.Instance.SetLog("제품 분류 중...");
                LogInit = true;
            }

            Thread.Sleep(12);
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
