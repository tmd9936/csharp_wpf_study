﻿using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.Model;
using FactorySimulation.Utill;

namespace FactorySimulation.Work
{
    public class AlignWorkThread : WorkThread
    {
        public AlignWorkThread( ProgressBar _progressBar,  TextBlock _box,  WorkThread _nextWorkThread)
            : base( _progressBar,  _box,  _nextWorkThread)
        {
            LogManager.Instance.SetLog("제품 스캔중...");
        }

        protected override void Act()
        {
            Thread.Sleep(1000);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 25;
            }));
            LogManager.Instance.SetLog("틀어짐 확인중...");
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 55;
            }));
            Thread.Sleep(1000);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 85;
            }));
            LogManager.Instance.SetLog("틸트 조정중...");
            Thread.Sleep(1000);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 100;
            }));
            LogManager.Instance.SetLog("어라인 완료");
            Thread.Sleep(200);

            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                if (nextWorkThread != null)
                {
                    PassObjectNextWorkThread();
                }
                else
                {
                    WorkEndInit();
                }
            }));
        }
    }
}
