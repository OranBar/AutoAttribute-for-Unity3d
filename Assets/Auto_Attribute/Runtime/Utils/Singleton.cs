using System;
using System.Linq;
using UnityEngine;

namespace Auto.Utils
{
	public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
	{
		private static T _instance;
		public static T Instance
		{
			get
			{
				if (_instance == null)
				{
					T[] instancesInScene = GameObjectEx.FindAllOfType<T>();
					if (instancesInScene.Length >= 2)
					{
						throw new Exception("Found " + instancesInScene.Length + " instances of " + typeof(T).Name + ", who is supposed to be a Singleton");
					}
					if (instancesInScene.FirstOrDefault() == null)
					{
						throw new Exception("Found 0 instances of " + typeof(T).Name + ". Please add one to the scene");
					}
					_instance = instancesInScene.First();
					_instance.InitTon();
				}
				return _instance;
		}
		}

		public void Awake()
		{
			if (_instance != null && _instance != this)
			{
				Debug.LogError("Two instances of the singleton " + typeof(T).Name, this.gameObject);
				Debug.LogError("Two instances of the singleton " + typeof(T).Name, Instance.gameObject);
				return;
			}

			if (_instance == null)
			{
				_instance = this as T;
				_instance.InitTon();
			}
		}

		/// <summary>
		/// Init Singleton
		/// </summary>
		protected abstract void InitTon();
	}
}