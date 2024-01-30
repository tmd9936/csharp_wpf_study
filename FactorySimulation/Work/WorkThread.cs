using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.PModel;

namespace FactorySimulation.Work
{
    public enum WORK_THREAD_TYPE
    {
        IN,
        OUT,
        ALIGN,
        BUFFER,
        CLASSITY,
    };

    public abstract class WorkThread
    {
        private const int WORKOFF_BRUSH_INDEX = 0;
        private const int WORKON_BRUSH_INDEX = 1;

        private readonly Thread thread;
        public readonly ProgressBar progressBar;
        protected readonly TextBlock box;
        protected readonly WorkThread nextWorkThread;
        protected readonly SolidColorBrush[] brush = new SolidColorBrush[2];
        protected object isWorkDoing;
        protected object IsComplete;

        protected WorkThread(ProgressBar _progressBar, TextBlock _box, Color _workOffColor, Color _workOnColor)
        {
            progressBar = _progressBar;
            box = _box;
            //nextWorkThread = _nextWorkThread;

            LogInit = false;
            IsEnd = false;
            IsPause = false;
            IsComplete = new bool();
            IsComplete = false;

            isWorkDoing = new bool();
            isWorkDoing = false;
            thread = new Thread(Job);

            brush[WORKOFF_BRUSH_INDEX] = new SolidColorBrush(_workOffColor);
            brush[WORKON_BRUSH_INDEX] = new SolidColorBrush(_workOnColor);

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
            if (product == null)
            {
                _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                {
                    progressBar.Value = 0;
                }));
            }

            IsPause = false;
        }

        public void WorkPause()
        {
            IsPause = true;
        }

        public bool GetIsCompelte()
        {
            lock (IsComplete)
            {
                return (bool)IsComplete;
            }
        }

        public bool GetIsWorkDoing()
        {
            lock (isWorkDoing)
            {
                return (bool)isWorkDoing;
            }
        }

        public bool ForceRemoval()
        {
            if (product == null)
            {
                return false;
            }

            product = null;

            lock (isWorkDoing)
            {
                isWorkDoing = false;
            }
            SetBoxWorkState(false);

            //_ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            //{
            //    progressBar.Value = 0;
            //}));

            return true;
        }

        public bool ForceInput(Product _product)
        {
            if (product != null || (bool)isWorkDoing || _product == null)
            {
                return false;
            }

            product = _product;

            lock (isWorkDoing)
            {
                isWorkDoing = true;
            }
            SetBoxWorkState(true);

            return true;
        }

        public bool TakeWork(Product _product)
        {
            if (product != null || (bool)isWorkDoing || _product == null)
            {
                return false;
            }

            lock (isWorkDoing)
            {
                if ((bool)isWorkDoing)
                {
                    return false;
                }

                isWorkDoing = true;
            }

            product = _product;
            SetBoxWorkState(true);

            return true;
        }

        protected void SetBoxWorkState(bool _isWorkOn)
        {
            _ = box.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                box.Background = _isWorkOn ? brush[WORKON_BRUSH_INDEX] : brush[WORKOFF_BRUSH_INDEX];
            }));
        }

        public virtual void WorkEndInit()
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
            product = null;
            lock (IsComplete)
            {
                IsComplete = false;
            }
        }

        protected abstract void Act();

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
                    {
                        continue;
                    }

                    Act();
                }
            }
        }

        public bool IsEnd { get; set; }
        public Product product { get; set; }
        protected bool LogInit { get; set; }
        private bool IsPause { get; set; }
    }
}
