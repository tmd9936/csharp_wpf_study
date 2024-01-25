using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using FactorySimulation.Work;
using FactorySimulation.Model;
using FactorySimulation.Utill;


namespace FactorySimulation.Service
{

    public class TransferService
    {
        public enum TRANSFER_STATE
        {
            INIT,
            WORKING,
            PAUSE,
            STOP
        }

        private static TransferService instence = null;

        private int CurProductNumber { get; set; }
        private List<WorkThread> threads = null;

        public static TransferService Instance
        {
            get
            {
                if (instence == null)
                {
                    instence = new TransferService();
                }
                return instence;
            }
        }

        private TransferService()
        {
            State = TRANSFER_STATE.INIT;
            CurProductNumber = 1000;
        }

        public void Initialize(List<ProgressBar> _progressBars, List<TextBlock> _boxes)
        {
            if (State != TRANSFER_STATE.INIT)
                return;

            threads = new List<WorkThread>();
            WorkThread curWorkThread;
            WorkThread nextWorkThread = new ClassifyWorkThread(
                _progressBars[_progressBars.Count - 1], _boxes[_progressBars.Count - 1], null);
            threads.Add(nextWorkThread);

            for (int i = _progressBars.Count - 2; i >= 0; --i)
            {
                if (i == 0)
                {
                    curWorkThread = new AlignWorkThread(_progressBars[i], _boxes[i], nextWorkThread);
                }
                else if (i == 1)
                {
                    curWorkThread = new BufferWorkThread(_progressBars[i], _boxes[i], nextWorkThread);
                }
                else
                {
                    curWorkThread = new ClassifyWorkThread(_progressBars[i], _boxes[i], nextWorkThread);
                }
                
                threads.Add(curWorkThread);
                nextWorkThread = curWorkThread;
            }

            threads.Reverse();

            State = TRANSFER_STATE.STOP;
        }

        public void WorkFinish()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].IsEnd = true;
            }
        }

        public void WorkStart()
        {
            State = TRANSFER_STATE.WORKING;

            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkStart();
            }

            LogManager.Instance.SetLog("공정 시작");
        }

        public bool WorkPuase()
        {
            if (State != TRANSFER_STATE.WORKING)
                return false;

            State = TRANSFER_STATE.PAUSE;

            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkPause();
            }

            LogManager.Instance.SetLog("공정 일시 정지");
            return true;
        }

        public void WorkStop()
        {
            State = TRANSFER_STATE.STOP;
            LogManager.Instance.SetLog("공정 정지");
        }

        public void ForceInput(int index)
        {
            if (State != TRANSFER_STATE.PAUSE)
                return;

            if (index < 0 || index >= threads.Count)
                return;

            if (threads[index] == null)
                return;

            Product product = new Product(CurProductNumber++);
            
            if (threads[index].ForceInput(product))
            {
                LogManager.Instance.SetLog(index + "번 공정에 물품 투입");
            }

        }

        public void ForceRemoval(int index)
        {
            if (State != TRANSFER_STATE.PAUSE)
                return;

            if (index < 0 || index >= threads.Count)
                return;

            if (threads[index] == null)
                return;

            if (threads[index].ForceRemoval())
            {
                LogManager.Instance.SetLog(index + "번 공정에 물품 제거");
            }
        }

        public void InputObject()
        {
            if (State != TRANSFER_STATE.WORKING)
                return;

            if (threads[0] == null)
                return;

            Product product = new Product(CurProductNumber++);
            threads[0].ForceInput(product);
        }

        private TRANSFER_STATE State { get; set; }
    }

}
