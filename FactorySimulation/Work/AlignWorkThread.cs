using System;
using System.Threading;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using FactorySimulation.PModel;
using FactorySimulation.Utill;

namespace FactorySimulation.Work
{
    public class AlignWorkThread : WorkThread
    {
        enum ALIGN_STATE
        {
            INIT,
            SCAN,
            CHECK,
            MOVE_TILT,
            COMPLETE
        }

        public AlignWorkThread(ProgressBar _progressBar, TextBlock _box, Color _workOffColor, Color _workOnColor) 
            : base(_progressBar, _box, _workOffColor, _workOnColor)
        {
            State = ALIGN_STATE.INIT;
        }

        protected override void Act()
        {
            Thread.Sleep(15);
            _ = progressBar.Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
            {
                if (progressBar.Maximum <= progressBar.Value)
                    return;

                progressBar.Value += 1;

                if (progressBar.Value == 1)
                {
                    LogManager.Instance.SetLog("제품 스캔중...");
                }

                if (progressBar.Value == 35)
                {
                    LogManager.Instance.SetLog("틀어짐 확인중...");
                }

                if (progressBar.Value == 70)
                {
                    LogManager.Instance.SetLog("틸트 조정중...");
                }

                if (progressBar.Value == progressBar.Maximum)
                {
                    LogManager.Instance.SetLog("어라인 완료");

                    lock (IsComplete)
                    {
                        IsComplete = true;
                    }
                }

            }));
        }

        private ALIGN_STATE State { get; set; }
    }
}
