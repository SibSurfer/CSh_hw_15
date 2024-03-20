class MyDisposable : IDisposable
{
    private bool _isDisposed = false;

    public int Value { get; set; }

    public void Dispose()
    {
        if (!_isDisposed)
        {
            _isDisposed = true;
            Console.WriteLine("Disposed " + Value);
        }
    }
}

class Cache<T> : IDisposable where T : IDisposable
{
    private readonly IDictionary<int, CacheEntry<T>> _buffer;
    private readonly int _capacity;
    private readonly long _delta;
    private readonly object _lock = new object();

    public Cache(int capacity, long delta)
    {
        _buffer = new Dictionary<int, CacheEntry<T>>();
        _capacity = capacity;
        _delta = delta;
    }

    public int Add(T value)
    {
        lock (_lock)
        {
            Clean();
            if (_buffer.Count == _capacity)
            {
                throw new Exception("Failed cleaning");
            }
            var hash = value.GetHashCode();
            _buffer.Add(hash, new CacheEntry<T> { Data = value, LastAccessTime = DateTimeOffset.Now.ToUnixTimeMilliseconds() });
            return hash;
        }
    }

    public T Get(int hash)
    {
        lock (_lock)
        {
            var entry = _buffer[hash];
            entry.LastAccessTime = DateTimeOffset.Now.ToUnixTimeMilliseconds();
            return entry.Data;
        }
    }

    private void Clean()
    {
        var time = DateTimeOffset.Now.ToUnixTimeMilliseconds();
        var keysToRemove = _buffer.Where(pair => (time - pair.Value.LastAccessTime) > _delta).Select(pair => pair.Key).ToList();
        foreach (var key in keysToRemove)
        {
            _buffer[key].Data.Dispose();
            _buffer.Remove(key);
        }
    }

    public void Dispose()
    {
        lock (_lock)
        {
            foreach (var entry in _buffer.Values)
            {
                entry.Data.Dispose();
            }
            _buffer.Clear();
        }
    }
}

class CacheEntry<T>
{
    public T Data { get; set; }
    public long LastAccessTime { get; set; }
}

class Program
{
    static void Main()
    {
        using (var obj1 = new MyDisposable { Value = 1 })
        using (var obj2 = new MyDisposable { Value = 2 })
        using (var obj3 = new MyDisposable { Value = 3 })
        {
            var cache = new Cache<MyDisposable>(2, 10);
            var hash1 = cache.Add(obj1);
            Console.WriteLine(cache.Get(hash1).Value);
            Thread.Sleep(100);
            var hash2 = cache.Add(obj2);
            Console.WriteLine(cache.Get(hash2).Value);
            Thread.Sleep(100);
            var hash3 = cache.Add(obj3);
        }
    }
}
