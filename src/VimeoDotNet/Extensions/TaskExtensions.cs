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
#if NET45
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run<TResult>(() =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                return task();
            });
#else
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            return Task.Run<TResult>(() =>
            {
                if (culture != null) CultureInfo.CurrentCulture = culture;
                if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
                return task();
            });
#endif
        }

        public static Task<TResult> RunWithCulture<TResult>(Func<Task<TResult>> task)
        {
#if NET45
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run<TResult>(async () =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                return await task().KeepCulture();
            });
#else
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            return Task.Run<TResult>(async () =>
            {
                if (culture != null) CultureInfo.CurrentCulture = culture;
                if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
                return await task().KeepCulture();
            });
#endif
        }

        public static Task RunWithCulture<TResult>(Action task)
        {
#if NET45
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run(() =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                task();
            });
#else
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            return Task.Run(() =>
            {
                if (culture != null) CultureInfo.CurrentCulture = culture;
                if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
                task();
            });
#endif
        }

        public static Task RunWithCulture(Func<Task> task)
        {
#if NET45
            var culture = Thread.CurrentThread.CurrentCulture;
            var uiCulture = Thread.CurrentThread.CurrentUICulture;
            return Task.Run(async () =>
            {
                var t = Thread.CurrentThread;
                if (culture != null) t.CurrentCulture = culture;
                if (uiCulture != null) t.CurrentUICulture = uiCulture;
                await task().KeepCulture();
            });
#else
            var culture = CultureInfo.CurrentCulture;
            var uiCulture = CultureInfo.CurrentUICulture;
            return Task.Run(async () =>
            {
                if (culture != null) CultureInfo.CurrentCulture = culture;
                if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
                await task().KeepCulture();
            });
#endif
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
            culture = CultureInfo.CurrentCulture;
            uiCulture = CultureInfo.CurrentUICulture;
            waiter.OnCompleted(continuation);
        }

        public void GetResult()
        {
#if NET45
            var t = Thread.CurrentThread;
            if (culture != null) t.CurrentCulture = culture;
            if (uiCulture != null) t.CurrentUICulture = uiCulture;
            waiter.GetResult();
#else
            if (culture != null) CultureInfo.CurrentCulture = culture;
            if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
            waiter.GetResult();
#endif
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
            culture = CultureInfo.CurrentCulture;
            uiCulture = CultureInfo.CurrentUICulture;
            waiter.OnCompleted(continuation);
        }

        public TResult GetResult()
        {
#if NET45
            var t = Thread.CurrentThread;
            if (culture != null) t.CurrentCulture = culture;
            if (uiCulture != null) t.CurrentUICulture = uiCulture;
            return waiter.GetResult();
#else
            if (culture != null) CultureInfo.CurrentCulture = culture;
            if (uiCulture != null) CultureInfo.CurrentUICulture = uiCulture;
            return waiter.GetResult();
#endif
        }
    }
}
