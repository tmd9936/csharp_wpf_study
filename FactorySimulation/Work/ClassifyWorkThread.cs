﻿using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.Model;
namespace FactorySimulation.Work
{
    public class ClassifyWorkThread : WorkThread
    {
        public ClassifyWorkThread(ProgressBar _progressBar, TextBlock _box, WorkThread _nextWorkThread)
            : base(_progressBar, _box, _nextWorkThread)
        {

        }

        protected override void Act()
        {
            Thread.Sleep(100);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value += 5;
                if (progressBar.Maximum > progressBar.Value)
                    return;

                if (nextWorkThread != null)
                {
                    PassObjectNextWorkThread();
                }
                else
                {
                    lock (isWorkDoing)
                    {
                        isWorkDoing = false;
                    }
                    SetBoxWorkState(false);
                    progressBar.Value = 0;
                }
            }));
        }
    }
}