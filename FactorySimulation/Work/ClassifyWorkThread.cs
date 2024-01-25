﻿using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.Model;
using FactorySimulation.Utill;

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
            if (!LogInit)
            {
                LogManager.Instance.SetLog("제품 분류 중...");
                LogInit = true;
            }

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
                    WorkEndInit();
                    LogManager.Instance.SetLog("제품 배출");
                }
            }));
        }

        private bool LogInit { get; set; }
    }
}
