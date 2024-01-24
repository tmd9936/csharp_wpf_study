using System;
using System.Collections.Generic;
using System.Windows.Controls;


namespace WpfApplication1.Utill
{
    public class WorkThreadManager
    {
        private const int randomSpeedValue = 7;
        private const int randomSpeedMin = 7;

        private readonly List<WorkThread> threads = null;
        public WorkThreadManager(ref List<ProgressBar> _progressBars, ref List<TextBlock> _boxes)
        {
            Random random = new Random(DateTime.Today.Millisecond);

            threads = new List<WorkThread>();
            WorkThread curWorkThread;
            WorkThread nextWorkThread = new WorkThread(
                _progressBars[_progressBars.Count - 1], _boxes[_progressBars.Count - 1],
                null, false,
                (random.Next() % randomSpeedValue) + randomSpeedMin);
            threads.Add(nextWorkThread);

            for (int i = _progressBars.Count - 2; i >= 0; --i)
            {
                curWorkThread = new WorkThread(
                    _progressBars[i], _boxes[i],
                    nextWorkThread, false,
                    (random.Next() % randomSpeedValue) + randomSpeedMin);
                threads.Add(curWorkThread);
                nextWorkThread = curWorkThread;
            }

            threads.Reverse();
        }

        public void WorkingFinish()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].IsEnd = true;
            }
        }

        public void WorkStart()
        {
            threads[0].IsAutoStart = true;
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkStart();
            }
        }

        public void WorkPuase()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkPause();
            }
        }

        public void WorkStop()
        {
            threads[0].IsAutoStart = false;
        }

        public void ForceStop(int index)
        {
            if (index < 0 || index >= threads.Count)
                return;

            if (threads[index] == null)
                return;

            threads[index].ForceStop();
        }
    }
}
