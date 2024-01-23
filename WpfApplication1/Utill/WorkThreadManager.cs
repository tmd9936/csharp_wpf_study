using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;


namespace WpfApplication1.Utill
{
    public class WorkThreadManager
    {
        const int randomSpeedValue = 7;
        const int randomSpeedMin = 7;

        private List<WorkThread> threads = null;
        public WorkThreadManager(List<ProgressBar> _progressBars, List<TextBlock> _boxes)
        {
            Random random = new Random(DateTime.Today.Millisecond);

            threads = new List<WorkThread>();
            WorkThread curWorkThread;
            WorkThread nextWorkThread = new WorkThread(_progressBars[_progressBars.Count - 1], _boxes[_progressBars.Count - 1], 
                null, false, random.Next() % randomSpeedValue + randomSpeedMin);
            threads.Add(nextWorkThread);

            for (int i = _progressBars.Count - 2; i >= 0; --i)
            {
                curWorkThread = new WorkThread(_progressBars[i], _boxes[i], 
                    nextWorkThread, false, random.Next() % randomSpeedValue + randomSpeedMin);
                threads.Add(curWorkThread);
                nextWorkThread = curWorkThread;
            }

            threads.Reverse();
        }

        public void WorkingFinish()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].isEnd = true;
            }
        }

        public void WorkStart()
        {
            threads[0].isAutoStart = true;
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
            threads[0].isAutoStart = false;
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
