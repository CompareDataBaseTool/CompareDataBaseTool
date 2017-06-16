using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace CompareDataBaseTool
{
    public class ProcessOperator
    {
        private BackgroundWorker _backgroundWorker;//后台线程
        private ProcessForm _processForm;//进度条窗体
        private BackgroundWorkerEventArgs _eventArgs;//异常参数
        public ProcessOperator()
        {
            _backgroundWorker = new BackgroundWorker();
            _processForm = new ProcessForm();
            _eventArgs = new BackgroundWorkerEventArgs();
            _backgroundWorker.DoWork += new DoWorkEventHandler(_backgroundWorker_DoWork);
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(_backgroundWorker_RunWorkerCompleted);
        }

        //操作进行完毕后关闭进度条窗体
        private void _backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_processForm.Visible == true)
            {
                _processForm.Close();
            }
            if (this.BackgroundWorkerCompleted != null)
            {
                this.BackgroundWorkerCompleted(null, _eventArgs);
            }
        }

        //后台执行的操作
        private void _backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            if (BackgroundWork != null)
            {
                try
                {
                    BackgroundWork();
                }
                catch (Exception ex)
                {
                    _eventArgs.BackGroundException = ex;
                }
            }
        }

        #region 公共方法、属性、事件

        /// <summary>
        /// 后台执行的操作
        /// </summary>
        public Action BackgroundWork { get; set; }

        /// <summary>
        /// 设置进度条显示的提示信息
        /// </summary>
        public string MessageInfo
        {
            set { _processForm.MessageInfo = value; }
        }
        /// <summary>
        /// 后台任务执行完毕后事件
        /// </summary>
        public event EventHandler<BackgroundWorkerEventArgs> BackgroundWorkerCompleted;

        /// <summary>
        /// 开始执行
        /// </summary>
        public void Start()
        {
            _backgroundWorker.RunWorkerAsync();
            _processForm.ShowDialog();
        }

        #endregion
    }
    public class BackgroundWorkerEventArgs : EventArgs
    {
        /// <summary>
        /// 后台程序运行时抛出的异常
        /// </summary>
        public Exception BackGroundException { get; set; }
    }
}