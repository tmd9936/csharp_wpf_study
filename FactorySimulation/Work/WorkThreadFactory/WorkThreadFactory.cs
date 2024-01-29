using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FactorySimulation.Work;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace FactorySimulation.Work
{
    public class WorkThreadFactory
    {
        private WorkThreadFactory()
        {}

        static public WorkThread MakeWorkThread(WORK_THREAD_TYPE type, ProgressBar _progressBar, TextBlock _box)
        {
            WorkThread workThread = null;

            switch (type)
            {
                case WORK_THREAD_TYPE.IN:
                    workThread = new InWorkThread(_progressBar, _box, Colors.Gray, Colors.LightGray);
                    break;
                case WORK_THREAD_TYPE.OUT:
                    workThread = new OutWorkThread(_progressBar, _box, Colors.BurlyWood, Colors.Brown);
                    break;
                case WORK_THREAD_TYPE.ALIGN:
                    workThread = new AlignWorkThread(_progressBar, _box, Colors.RosyBrown, Colors.Navy);
                    break;
                case WORK_THREAD_TYPE.BUFFER:
                    workThread = new BufferWorkThread(_progressBar, _box, Colors.RosyBrown, Colors.Navy);
                    break;
                case WORK_THREAD_TYPE.CLASSITY:
                    workThread = new ClassifyWorkThread(_progressBar, _box, Colors.RosyBrown, Colors.Navy);
                    break;
                default:
                    break;
            }

            return workThread;
        }
    }
}
