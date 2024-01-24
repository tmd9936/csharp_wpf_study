using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;


namespace WpfApplication1.Utill
{
    public class WorkThread
    {
        private const int WORKOFF_BRUSH_INDEX = 0;
        private const int WORKON_BRUSH_INDEX = 1;

        private readonly Thread thread;
        private readonly ProgressBar progressBar;
        private readonly TextBlock box;
        private readonly WorkThread nextWorkThread;
        private readonly int speed;
        private readonly SolidColorBrush[] brush;

        private object isWorkDoing;

        public WorkThread(ProgressBar _progressBar, TextBlock _box,
            WorkThread _nextWorkThread, bool _isAutoStart, int _speed)
        {
            progressBar = _progressBar;
            box = _box;
            nextWorkThread = _nextWorkThread;
            IsAutoStart = _isAutoStart;
            speed = _speed;

            IsEnd = false;
            IsPause = false;

            isWorkDoing = new bool();
            isWorkDoing = false;
            thread = new Thread(Job);

            brush = new SolidColorBrush[2];
            brush[WORKOFF_BRUSH_INDEX] = new SolidColorBrush(Colors.RosyBrown);
            brush[WORKON_BRUSH_INDEX] = new SolidColorBrush(Colors.Navy);

            box.Background = brush[WORKOFF_BRUSH_INDEX];

            thread.Start();
        }

        ~WorkThread()
        {
            IsEnd = true;
            thread.Join();
        }

        public void WorkStart()
        {
            IsPause = false;
        }

        public void WorkPause()
        {
            IsPause = true;
        }

        public bool TakeWork()
        {
            lock (isWorkDoing)
            {
                if ((bool)isWorkDoing)
                    return false;

                isWorkDoing = true;
            }
            SetBoxWorkState(true);

            return true;
        }

        public void ForceStop()
        {
            lock (isWorkDoing)
            {
                isWorkDoing = false;
            }
            SetBoxWorkState(false);

            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 0;
            }));
        }

        private void Job()
        {
            bool _isWorkDoing = false;
            while (!IsEnd)
            {
                lock (isWorkDoing)
                {
                    _isWorkDoing = (bool)isWorkDoing;
                }

                if (_isWorkDoing)
                {
                    if (IsPause)
                        continue;

                    Thread.Sleep(100);
                    _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        progressBar.Value += speed;
                        if (progressBar.Maximum > progressBar.Value)
                            return;

                        if (nextWorkThread != null)
                        {
                            if (nextWorkThread.TakeWork())
                            {
                                SetBoxWorkState(false);
                                progressBar.Value = 0;
                                lock (isWorkDoing)
                                {
                                    isWorkDoing = false;
                                }
                            }
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
                else
                {
                    if (IsAutoStart)
                    {
                        if (IsPause)
                            continue;

                        Thread.Sleep(200);
                        lock (isWorkDoing)
                        {
                            isWorkDoing = true;
                        }
                        SetBoxWorkState(true);
                    }
                }
            }
        }

        private void SetBoxWorkState(bool _isWorkOn)
        {
            _ = box.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                if (_isWorkOn)
                    box.Background = brush[WORKON_BRUSH_INDEX];
                else
                    box.Background = brush[WORKOFF_BRUSH_INDEX];
            }));
        }

        public bool IsEnd { get; set; }
        public bool IsAutoStart { get; set; }
        private bool IsPause { get; set; }
    }
}
