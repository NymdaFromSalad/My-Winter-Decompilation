using System;
using System.Collections;
using System.Collections.Generic;

namespace Boo.Lang
{
	public abstract class GenericGenerator<T> : IEnumerable<T>
	{
		public abstract IEnumerator<T> GetEnumerator();
		
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}
	
	public class GenericGeneratorEnumerator<T> : IEnumerator<T>
	{
		protected IEnumerator<T> _inner;
		
		// REQUIRED: parameterless ctor (UnityScript-generated classes use this)
		public GenericGeneratorEnumerator()
		{
		}
		
		// Optional wrapper ctor
		public GenericGeneratorEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}
		
		public virtual T Current
		{
			get { return _inner != null ? _inner.Current : default(T); }
		}
		
		object IEnumerator.Current
		{
			get { return Current; }
		}
		
		public virtual bool MoveNext()
		{
			return _inner != null && _inner.MoveNext();
		}
		
		public virtual void Reset()
		{
			if (_inner != null)
				_inner.Reset();
		}
		
		public virtual void Dispose()
		{
			if (_inner != null)
				_inner.Dispose();
		}
	}
}
