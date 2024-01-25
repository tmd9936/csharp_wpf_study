using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.Model;

namespace FactorySimulation.Work
{
    public abstract class WorkThread
    {
        private const int WORKOFF_BRUSH_INDEX = 0;
        private const int WORKON_BRUSH_INDEX = 1;

        private readonly Thread thread;
        protected readonly ProgressBar progressBar;
        protected readonly TextBlock box;
        protected readonly WorkThread nextWorkThread;
        protected readonly SolidColorBrush[] brush;
        protected object isWorkDoing;

        protected WorkThread(ProgressBar _progressBar, TextBlock _box, WorkThread _nextWorkThread)
        {
            progressBar = _progressBar;
            box = _box;
            nextWorkThread = _nextWorkThread;

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

        public bool ForceRemoval()
        {
            if (product == null)
                return false;

            product = null;

            lock (isWorkDoing)
            {
                isWorkDoing = false;
            }
            SetBoxWorkState(false);

            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                progressBar.Value = 0;
            }));

            return true;
        }

        public bool ForceInput(Product _product)
        {
            if (product != null || (bool)isWorkDoing || _product == null)
                return false;

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
            lock (isWorkDoing)
            {
                if ((bool)isWorkDoing)
                    return false;

                isWorkDoing = true;
            }

            product = _product;
            SetBoxWorkState(true);

            return true;
        }

        protected bool PassObjectNextWorkThread()
        {
            if (nextWorkThread == null || product == null)
                return false;

            if (nextWorkThread.TakeWork(product))
            {
                WorkEndInit();

                return true;
            }

            return false;
        }

        protected void SetBoxWorkState(bool _isWorkOn)
        {
            _ = box.Dispatcher.BeginInvoke(DispatcherPriority.Normal, new Action(delegate
            {
                box.Background = _isWorkOn ? brush[WORKON_BRUSH_INDEX] : brush[WORKOFF_BRUSH_INDEX];
            }));
        }

        protected void WorkEndInit()
        {
            lock (isWorkDoing)
            {
                isWorkDoing = false;
            }
            SetBoxWorkState(false);
            progressBar.Value = 0;
            product = null;
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
                        continue;

                    Act();
                }
            }
        }

        public bool IsEnd { get; set; }
        public Product product { get; set; }
        private bool IsPause { get; set; }
    }
}
