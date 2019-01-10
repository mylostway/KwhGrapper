using KwhGrapper.core;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace KwhGrapper
{
    /// <summary>
    /// App.xaml 的交互逻辑
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            //UI线程未捕获异常处理事件
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;

            //Task线程内未捕获异常处理事件
            TaskScheduler.UnobservedTaskException += TaskScheduler_UnobservedTaskException;

            //非UI线程未捕获异常处理事件
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("系统出错了，非UI线程未捕获异常处理事件，原因：{0}", e.ExceptionObject.ToString()));
            SLogger.Instance.LogErr(e.ExceptionObject.ToString());
            Application.Current.Shutdown(-1);
        }

        private void TaskScheduler_UnobservedTaskException(object sender, UnobservedTaskExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("系统出错了，Task线程内未捕获异常处理事件，原因：{0}", e.Exception.Message));
            SLogger.Instance.LogErr(e.Exception.ToString(), e.Exception);
            Application.Current.Shutdown(-1);
        }

        private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
        {
            MessageBox.Show(string.Format("系统出错了，UI线程未捕获异常处理事件，原因：{0}", e.Exception.Message));
            //e.Handled = true;
            SLogger.Instance.LogErr(e.Exception.ToString(), e.Exception);
            Application.Current.Shutdown(-1);
        }
    }
}
