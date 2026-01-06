using System;
using System.Collections;
using System.Collections.Generic;

namespace UnityScript.Lang
{
	// -------------------------
	// Array helpers
	// -------------------------
	public static class Array
	{
		public static int get_length(System.Array a)
		{
			return a.Length;
		}
	}
	
	// -------------------------
	// Builtins
	// -------------------------
	public static class Builtins
	{
		public static bool op_Implicit(object o)
		{
			return o != null;
		}
	}
	
	// -------------------------
	// Coroutine generators
	// -------------------------
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
		private IEnumerator<T> _inner;
		
		public GenericGeneratorEnumerator(IEnumerator<T> inner)
		{
			_inner = inner;
		}
		
		public T Current
		{
			get { return _inner.Current; }
		}
		
		object IEnumerator.Current
		{
			get { return Current; }
		}
		
		public bool MoveNext()
		{
			return _inner.MoveNext();
		}
		
		public void Reset()
		{
			_inner.Reset();
		}
		
		public void Dispose()
		{
			_inner.Dispose();
		}
	}
}
