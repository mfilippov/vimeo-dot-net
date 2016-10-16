using System;
using System.Globalization;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;

namespace VimeoDotNet.Extensions
{
    internal static class TaskExtensions
    {
        public static TResult RunSynchronouslyWithCurrentCulture<TResult>(this Task<TResult> task)
        {
            return RunWithCulture(async () => await task.KeepCulture())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public static void RunSynchronouslyWithCurrentCulture(this Task task)
        {
            RunWithCulture(async () => await task.KeepCulture())
                .ConfigureAwait(false)
                .GetAwaiter()
                .GetResult();
        }

        public static PreserveCultureAwaiter<TResult> KeepCulture<TResult>(this Task<TResult> task)
        {
            return new PreserveCultureAwaiter<TResult>(task.GetAwaiter());
        }

        public static PreserveCultureAwaiter KeepCulture(this Task task)
        {
            return new PreserveCultureAwaiter(task.GetAwaiter());
        }

        public static Task<TResult> RunWithCulture<TResult>(Func<TResult> task)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run<TResult>(() =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                return task();
            });
        }

        public static Task<TResult> RunWithCulture<TResult>(Func<Task<TResult>> task)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run<TResult>(async () =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                return await task().KeepCulture();
            });
        }

        public static Task RunWithCulture<TResult>(Action task)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                task();
            });
        }

        public static Task RunWithCulture(Func<Task> task)
        {
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run(async () =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                await task().KeepCulture();
            });
        }
    }

    internal class PreserveCultureAwaiter : INotifyCompletion
    {
        private TaskAwaiter waiter;
        private CultureInfo culture;
        private CultureInfo uiCulture;

        public PreserveCultureAwaiter(TaskAwaiter waiter)
        {
            this.waiter = waiter;
        }

        public PreserveCultureAwaiter GetAwaiter() { return this; }

        public bool IsCompleted { get { return waiter.IsCompleted; } }

        public void OnCompleted(Action continuation)
        {
            var t = Thread.CurrentThread;
            culture = t.CurrentCulture;
            uiCulture = t.CurrentUICulture;
            waiter.OnCompleted(continuation);
        }

        public void GetResult()
        {
            var t = Thread.CurrentThread;
            if (culture != null) t.CurrentCulture = culture;
            if (uiCulture != null) t.CurrentUICulture = uiCulture;
            waiter.GetResult();
        }
    }

    internal class PreserveCultureAwaiter<TResult> : INotifyCompletion
    {
        private TaskAwaiter<TResult> waiter;
        private CultureInfo culture;
        private CultureInfo uiCulture;

        public PreserveCultureAwaiter(TaskAwaiter<TResult> waiter)
        {
            this.waiter = waiter;
        }

        public PreserveCultureAwaiter<TResult> GetAwaiter() { return this; }

        public bool IsCompleted { get { return waiter.IsCompleted; } }

        public void OnCompleted(Action continuation)
        {
            var t = Thread.CurrentThread;
            culture = t.CurrentCulture;
            uiCulture = t.CurrentUICulture;
            waiter.OnCompleted(continuation);
        }

        public TResult GetResult()
        {
            var t = Thread.CurrentThread;
            if (culture != null) t.CurrentCulture = culture;
            if (uiCulture != null) t.CurrentUICulture = uiCulture;
            return waiter.GetResult();
        }
    }
}
