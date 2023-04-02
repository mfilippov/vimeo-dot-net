// write simple http server to test requests

using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace VimeoDotNet.Tests;

internal class TestHttpServer
{
    private readonly HttpListener _listener;
    private Thread _workerThread;
    private readonly object _myLock = new();
    private volatile bool _started;
    private volatile bool _shutdownRequested;
    private Exception _innerException;

    public TestHttpServer()
    {
        _listener = new HttpListener();
    }

    private static int GetFreePort()
    {
        var tcp = new TcpListener(IPAddress.Any, 0);
        tcp.Start();
        var port = ((IPEndPoint)tcp.LocalEndpoint).Port;
        tcp.Stop();
        return port;
    }

    public int Start(ConcurrentDictionary<string, Action<HttpListenerRequest, HttpListenerResponse>> handlers)
    {
        lock (_myLock)
        {
            if (_started)
            {
                throw new Exception("Server already started");
            }

            _started = true;
        }

        var port = GetFreePort();
        _listener.Prefixes.Add($"http://localhost:{port}/");
        _listener.Start();
        _workerThread = new Thread(() =>
        {
            while (!_shutdownRequested)
            {
                HttpListenerContext ctx = null;
                try
                {
                    ctx = _listener.GetContext();
                    var handlerFound =
                        handlers.TryGetValue($"{ctx.Request.Url?.PathAndQuery ?? ""}:{ctx.Request.HttpMethod}",
                            out var handler);
                    if (!handlerFound)
                    {
                        throw new Exception(
                            $"url handler not found for suffix: '{ctx.Request.Url?.PathAndQuery}' " +
                            $"and method: {ctx.Request.HttpMethod}");
                    }

                    handler(ctx.Request, ctx.Response);

                    if (_shutdownRequested)
                    {
                        break;
                    }
                }
                catch (Exception ex)
                {
                    if (!_shutdownRequested)
                    {
                        _innerException = ex;
                        if (ctx != null)
                        {
                            ctx.Response.StatusCode = 500;
                            var wrt = new StreamWriter(ctx.Response.OutputStream);
                            wrt.Write($$"""{ "error": "Internal server error" }""");
                            wrt.Close();
                            ctx.Response.Close();
                        }
                    }

                    break;
                }
            }
        });
        _workerThread.Start();
        return port;
    }

    public void Stop()
    {
        _shutdownRequested = true;
        _listener.Stop();
        if (_workerThread?.IsAlive ?? false)
        {
            _workerThread.Join();
        }

        if (_innerException != null)
        {
            throw _innerException;
        }
    }
}