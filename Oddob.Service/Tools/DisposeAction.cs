namespace Oddob.Service.Tools;

internal sealed class DisposeAction : IDisposable
{
    public static DisposeAction Action(Action action)
    {
        DisposeAction da = new();
        da.Add(action);
        return da;
    }

    private readonly Queue<Action> _actionQueue = new Queue<Action>();

    /// <summary>
    /// 添加动作
    /// </summary>
    /// <param name="action"></param>
    public void Add(Action action)
    {
        _actionQueue.Enqueue(action);
    }

    private bool _disposedValue;

    private void Dispose(bool disposing)
    {
        if (_disposedValue)
            return;
        if (disposing)
        {
            while (_actionQueue.Count != 0)
            {
                _actionQueue.Dequeue().Invoke();
            }
        }

        // 释放未托管的资源(未托管的对象)并重写终结器
        // 将大型字段设置为 null
        _disposedValue = true;
    }

    // 仅当“Dispose(bool disposing)”拥有用于释放未托管资源的代码时才替代终结器
    ~DisposeAction()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: false);
    }

    public void Dispose()
    {
        // 不要更改此代码。请将清理代码放入“Dispose(bool disposing)”方法中
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}