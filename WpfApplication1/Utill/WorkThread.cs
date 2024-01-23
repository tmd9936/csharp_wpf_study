using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApplication1.Utill
{
    public class WorkThread
    {
        private Thread thread;
        private ProgressBar progressBar;
        private TextBlock box;
        private WorkThread nextWorkThread;
        private int speed;
        
        public WorkThread(ProgressBar _progressBar, TextBlock _box, 
            WorkThread _nextWorkThread, bool _isAutoStart, int _speed)
        {
            progressBar = _progressBar;
            box = _box;
            nextWorkThread = _nextWorkThread;
            isAutoStart = _isAutoStart;
            speed = _speed;

            isEnd = false;
            isWorkDoing = false;
            isPause = false;

            box.Background = new SolidColorBrush(Colors.RosyBrown);
            thread = new Thread(Job);

            thread.Start();
        }

        ~WorkThread()
        {
            isEnd = true;
            thread.Join();
        }

        public void WorkStart()
        {
            isPause = false;
        }

        public void WorkPause()
        {
            isPause = true;
        }

        public bool TakeWork()
        {
            if (isWorkDoing)
                return false;

            isWorkDoing = true;
            SetBoxColor(Colors.Navy);

            return true;
        }
        public void ForceStop()
        {
            lock (this)
            {
                isWorkDoing = false;
            }
            SetBoxColor(Colors.RosyBrown);
            progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 0;
            }));
        }


        private void Job()
        {
 
            while (!isEnd)
            {
                if (isWorkDoing)
                {
                    if (isPause)
                        continue;

                    Thread.Sleep(100);
                    progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        progressBar.Value += speed;
                        if (progressBar.Maximum <= progressBar.Value)
                        {
                            if (nextWorkThread != null)
                            {
                                lock(nextWorkThread)
                                {
                                    if (nextWorkThread.TakeWork())
                                    {
                                        SetBoxColor(Colors.RosyBrown);
                                        progressBar.Value = 0;
                                        lock (this)
                                        {
                                            isWorkDoing = false;
                                        }
                                    }
                                }
                            }
                            else
                            {
                                lock (this)
                                {
                                    isWorkDoing = false;
                                }
                                SetBoxColor(Colors.RosyBrown);
                                progressBar.Value = 0;
                            }
                        }
                    }));

                }
                else
                {
                    if (isAutoStart)
                    {
                        if (isPause)
                            continue;

                        Thread.Sleep(200);
                        lock (this)
                        {
                            isWorkDoing = true;
                        }
                        SetBoxColor(Colors.Navy);
                    }
                }
            }
        }

        private void SetBoxColor(Color color)
        {
            box.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                box.Background = new SolidColorBrush(color);
            }));
        }

        public bool isWorkDoing { get; set; }
        public bool isEnd { get; set; }
        public bool isAutoStart { get; set; }
        private bool isPause { get; set; }
    }
}
