interface IPooleable<T>
{
	void Initialize(T Object);
	void Dispose(T Object);
}

