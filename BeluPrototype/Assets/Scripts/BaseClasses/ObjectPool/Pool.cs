using System;
using System.Collections.Generic;

public class Pool<T> {
	public Func<T> _FactoryMethod;

	private int _count;
	private bool _isDinamic = false;

	private Action<T> _init;
	private Action<T> _finit;
	private List<PoolObject<T>> _poolList;

	class PoolObject<R>
	{
		bool _isActive;
		R _obj;
		Action<R> _initializationCallback;
		Action<R> _finalizationCallback;

		//Constructor.
		public PoolObject(R obj, Action<R> init, Action<R> finit)
		{
			_obj = obj;
			_initializationCallback = init;
			_finalizationCallback = finit;
			_isActive = false;
		}

		public R GetObj
		{
			get { return _obj; }
		}

		public bool isActive
		{
			get { return _isActive; }
			set
			{
				_isActive = value;
				if (_isActive && _initializationCallback != null)
					_initializationCallback(_obj);
				else if (_finalizationCallback != null)
					_finalizationCallback(_obj);
			}
		}
	}

	//Constructor.
	public Pool(int initialStock, Func<T> FactoryMethod, Action<T> init, Action<T> finit, bool isDinamic = false)
	{
		_poolList = new List<PoolObject<T>>();
		_FactoryMethod = FactoryMethod;
		_isDinamic = isDinamic;
		_count = initialStock;
		_init = init;
		_finit = finit;

		for (int i = 0; i < _count; i++)
			_poolList.Add(new PoolObject<T>(_FactoryMethod(), _init, _finit));
	}

	public T GetObjectFromPool()
	{
		for (int i = 0; i < _count; i++)
			if (!_poolList[i].isActive)
			{
				_poolList[i].isActive = true;
				return _poolList[i].GetObj;
			}

		if (_isDinamic)
		{
			var po = new PoolObject<T>(_FactoryMethod(), _init, _finit);
			po.isActive = true;
			_poolList.Add(po);
			_count++;
			return po.GetObj;
		}
		return default(T);
	}

	public void DisablePoolObject(T obj)
	{
		foreach (var item in _poolList)
			if (item.GetObj.Equals(obj))
			{
				item.isActive = false;
				return;
			}
	}
}
