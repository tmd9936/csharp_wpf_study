using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Threading;
using System.Windows.Threading;
using FactorySimulation.Work;
using FactorySimulation.PModel;
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
            STOP,
            END
        }

        private static TransferService _instence = null;
        public static TransferService Instance
        {
            get
            {
                if (_instence == null)
                {
                    _instence = new TransferService();
                }
                return _instence;
            }
        }

        private TransferService()
        {
            State = TRANSFER_STATE.INIT;
            CurProductNumber = 1003;
        }

        public void Destroyed()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].IsEnd = true;
            }
            State = TRANSFER_STATE.END;

            if (AutoCycleThread != null)
                AutoCycleThread.Join();

        }

        public void Initialize(List<ProgressBar> _progressBars, List<TextBlock> _boxes)
        {
            if ((int)State != ((int)TRANSFER_STATE.INIT))
                return;

            threads = new List<WorkThread>();
            WorkThread curWorkThread;

            for (int i = 0; i < _progressBars.Count; ++i)
            {
                if (i == 0)
                {
                    curWorkThread = WorkThreadFactory.MakeWorkThread(WORK_THREAD_TYPE.IN, _progressBars[i], _boxes[i]);
                }
                else if (i == 1)
                {
                    curWorkThread = WorkThreadFactory.MakeWorkThread(WORK_THREAD_TYPE.ALIGN, _progressBars[i], _boxes[i]);
                }
                else if (i == 2 || i == 4)
                {
                    curWorkThread = WorkThreadFactory.MakeWorkThread(WORK_THREAD_TYPE.BUFFER, _progressBars[i], _boxes[i]);
                }
                else if (i == _progressBars.Count - 1)
                {
                    curWorkThread = WorkThreadFactory.MakeWorkThread(WORK_THREAD_TYPE.OUT, _progressBars[i], _boxes[i]);
                }
                else
                {
                    curWorkThread = WorkThreadFactory.MakeWorkThread(WORK_THREAD_TYPE.CLASSITY, _progressBars[i], _boxes[i]);
                }

                threads.Add(curWorkThread);
            }

            lock (State)
            {
                State = TRANSFER_STATE.STOP;
            }
        }

        private void AutoCycle()
        {
            bool needTransferMove = false;
            bool isWorkingThread = false;

            while ((int)State == ((int)TRANSFER_STATE.WORKING))
            {
                Thread.Sleep(100);

                isWorkingThread = false;
                needTransferMove = true;
                foreach (WorkThread wt in threads)
                {
                    if (wt.GetIsWorkDoing())
                    {
                        isWorkingThread = true;
                        if (wt.GetIsCompelte() == false)
                        {
                            needTransferMove = false;
                            continue;
                        }
                    }
                }

                if (isWorkingThread && needTransferMove)
                {
                    for (int i = threads.Count - 1; i >= 0; --i)
                    {
                        if (threads[i].GetIsCompelte())
                        {
                            threads[i].WorkEndInit();
                        }

                        if (i > 0 && threads[i - 1].GetIsCompelte())
                        {
                            threads[i].TakeWork(threads[i - 1].product);
                        }
                        
                    }
                }
            }
        }


        public void WorkStart()
        {
            lock (State)
            {
                State = TRANSFER_STATE.WORKING;
            }

            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkStart();
            }

            AutoCycleThread = new Thread(AutoCycle);
            AutoCycleThread.Start();

            LogManager.Instance.SetLog("공정 시작");
        }

        public bool WorkPuase()
        {
            if ((int)State != ((int)TRANSFER_STATE.WORKING))
                return false;

            lock (State)
            {
                State = TRANSFER_STATE.PAUSE;
            }

            for (int i = 0; i < threads.Count; ++i)
            {
                threads[i].WorkPause();
            }

            AutoCycleThread.Join();
            LogManager.Instance.SetLog("공정 일시 정지");
            return true;
        }

        public void WorkStop()
        {
            lock (State)
            {
                State = TRANSFER_STATE.STOP;
            }
            LogManager.Instance.SetLog("공정 정지");
        }

        public void ForceInput(string name, int index)
        {
            if ((int)State != ((int)TRANSFER_STATE.PAUSE))
                return;

            if (index < 0 || index >= threads.Count)
                return;

            if (threads[index] == null)
                return;

            //if (IsProductInFactoryLine()) // 1개 물품만 다시 넣기 가능?
            //    return;

            AutoCycleThread.Join();

            Product product = new Product() {ID= CurProductNumber++, IsOK = true, Name = name };
            
            if (threads[index].ForceInput(product))
            {
                LogManager.Instance.SetLog(index + "번 공정에 물품 투입");
            }

        }

        public void ForceRemoval(int index)
        {
            if ((int)State != ((int)TRANSFER_STATE.PAUSE))
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

        public void InputObject(string name)
        {
            if ((int)State != ((int)TRANSFER_STATE.WORKING))
                return;

            if (threads[0] == null)
                return;

            Product product = new Product() { ID = CurProductNumber++, IsOK = true, Name = name };
            threads[0].ForceInput(product);
        }

        public bool IsProductInFactoryLine()
        {
            for (int i = 0; i < threads.Count; ++i)
            {
                if (threads[i].product != null)
                {
                    return true;
                }
            }

            return false;
        }

        public void CycleProduct(int index)
        {
            if ((int)State != ((int)TRANSFER_STATE.PAUSE))
                return;

            if (index < 0 || index >= threads.Count)
                return;

            if (threads[index] == null)
                return;

            if (threads[index].product == null)
                return;

            threads[index].WorkStart();
        }

        private object State { get; set; }
        private int CurProductNumber { get; set; }
        private Thread AutoCycleThread { get; set; }
        private List<WorkThread> threads { get; set; }

    }

}
