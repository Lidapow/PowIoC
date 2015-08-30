using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Reflection;
using PowIoC;

public class Injector : ScriptableObject {
	[Inject]
	private IParsable parser;
	[Inject(false)]
	private ILogger logger;
	[Inject]
	private ISettingLoader loader;

	private InjectMap injectMap;

	private static Injector _instance;
	private static Injector instance {
		get {
			if(_instance == null){
				_instance = ScriptableObject.CreateInstance<Injector>();
				_instance.Read();
				Inject(_instance);
				_instance.ReadFromLoader();
			}
			return _instance;
		}
	}
	private Dictionary<Type, Dictionary<string, object>> singletonInstances;

	public static void Read (string settings) {
		instance._Read(settings);
	}

	private void Read () {
		injectMap = Resources.Load("InjectMap", typeof(InjectMap)) as InjectMap;
	}

	private void ReadFromLoader () {
		if(loader != null)
			_Read(loader.GetSettings());
	}

	public void _Read (string settings) {
		if(string.IsNullOrEmpty(settings))
			return;
			
		InjectMap outsideSettings = parser.Parse<InjectMap>(settings);
		if(outsideSettings == null)
			return;

		foreach(BindMap bindMapCurr in injectMap.bind){
			// If there hasn't setting, leave loop.
			if(outsideSettings.bind == null)
				break;

			BindMap bindMapNew = null;
			bindMapNew = Array.Find(
				outsideSettings.bind, 
				(BindMap bm) => bm.bind.Equals(bindMapCurr.bind)
				);

			if(bindMapNew != null){
				bindMapCurr.to = String.Copy(bindMapNew.to);
			}
		}

		foreach(PrimitiveMap primitiveMapCurr in injectMap.primitive){
			// If there hasn't setting, leave loop.
			if(outsideSettings.primitive == null)
				break;

			PrimitiveMap primitiveMapNew = null;
			primitiveMapNew = Array.Find(
				outsideSettings.primitive,
				(PrimitiveMap pm) => pm.fieldPath.Equals(primitiveMapCurr.fieldPath)
				);

			if(primitiveMapNew != null){
				primitiveMapCurr.fieldValue = String.Copy(primitiveMapNew.fieldValue);
			}
		}

		foreach(PrimitiveArrayMap primitiveArrayMapCurr in injectMap.primitiveArray){
			// If there hasn't setting, leave loop.
			if(outsideSettings.primitiveArray == null)
				break;

			PrimitiveArrayMap PrimitiveArrayMapNew = null;
			PrimitiveArrayMapNew = Array.Find(
				outsideSettings.primitiveArray,
				(PrimitiveArrayMap pam) => pam.fieldPath.Equals(primitiveArrayMapCurr.fieldPath)
				);

			if(PrimitiveArrayMapNew != null){
				// Expand size of array, when source array large than destination
				if(primitiveArrayMapCurr.fieldValue.Length != PrimitiveArrayMapNew.fieldValue.Length)
					primitiveArrayMapCurr.fieldValue = new string[PrimitiveArrayMapNew.fieldValue.Length];
				Array.Copy(PrimitiveArrayMapNew.fieldValue, primitiveArrayMapCurr.fieldValue, PrimitiveArrayMapNew.fieldValue.Length);
			}
		}

		//Due to new settings changed, reinject level of logger
		Inject(logger);
	}

	void OnEnable ()
	{
		singletonInstances = new Dictionary<Type, Dictionary<string, object>>();
	}

	void OnDisable()
	{
		singletonInstances = null;
	}

	public static T Get<T> () where T : ScriptableObject {
		return instance._Get<T>();
	}

	public static T New<T> () where T : ScriptableObject {
		return instance._New<T>();
	}

	public static ScriptableObject New (Type type) {
		return instance._New(type);
	}

	public static ScriptableObject New (Type type, string scope) {
		return instance._New(type, scope);
	}

	public static ScriptableObject Get (Type type) {
		return instance._Get(type);
	}

	public static ScriptableObject Get (Type type, string scope) {
		return instance._Get(type, scope);
	}

	/// <summary>
	/// Inject instance to field what noted with Inject attribute
	/// </summary>
	/// <example>
	/// <code>
	///
	/// public class StatusBarController : AbstractStatusBarController {
	///		public Camera myCamera;
	///		private bool enableCamera;
	///		[Inject(singleton = false)]		//
	///		public string infoStr;			// These fields can be assigned when the InstanceManager.asset has specified value
	///		[Inject(singleton = false)]		// Or it is default value when on specified value in InstanceManager.asset
	///		public int gameNum;				//
	///		[Inject(scope="Something")]
	///		public ICommand cmdFirst;
	///		[Inject(scope="SomeElse")]
	///		public ICommand cmdSecond;
	/// </code>
	/// </example>
	public static T Get <T> (Type type, string fieldName) {
		return instance._Get <T> (type, fieldName);
	}

	public T _Get <T> (Type type, string fieldName) {
		PrimitiveMap map = Array.Find<PrimitiveMap>(injectMap.primitive, (x) => 
			x.fieldPath.Equals(string.Format("{0}.{1}", type.ToString(), fieldName)));	

		if (map != null) 
		{
			Log(string.Format("<Get>Found path {0}, for field {2} of type {1} to value {3}", map.fieldPath, type.ToString(), fieldName, map.fieldValue));
			return (T) Convert.ChangeType(map.fieldValue, typeof(T));
		} else {
			throw new PrimitiveMapNotDeclareException();
		}
	}

	public T[] _GetArray<T> (Type type, string fieldName) {
		PrimitiveArrayMap map = Array.Find<PrimitiveArrayMap>(injectMap.primitiveArray, (x) =>
			x.fieldPath.Equals(string.Format("{0}.{1}", type.ToString(), fieldName)));

		if (map != null)
		{
			Log(string.Format("<GetArray>Found path {0}, for field {2} of type {1} to value {3}", map.fieldPath, type.ToString(), fieldName, map.fieldValue));
			T[] t = new T[map.fieldValue.Length];
			for(int i = 0; i < map.fieldValue.Length; i++){
				t[i] = (T) Convert.ChangeType(map.fieldValue[i], typeof(T));
			}
			return t;
		} else {
			throw new PrimitiveMapNotDeclareException();
		}
	}

	public T _Get<T> (string scope = "") where T : ScriptableObject {
		T t;
		Type type = typeof(T);
		if(!singletonInstances.ContainsKey(type)){
			singletonInstances.Add(type, new Dictionary<string, object>());
		}
		if(!singletonInstances[type].ContainsKey(scope)){
			t = ScriptableObject.CreateInstance<T>();
			singletonInstances[type].Add(scope, t);
			logger.Log("InstanceManager create scriptable object " + type);
		}
		t = (T)singletonInstances[type][scope];

		return t;
	}

	public ScriptableObject _Get (Type type, string scope = "") {
		//TODO scope singleton
		ScriptableObject t;
		if(!singletonInstances.ContainsKey(type)){
			singletonInstances.Add(type, new Dictionary<string, object>());
		}
		if(!singletonInstances[type].ContainsKey(scope)){
			Log("Going to new for " + type + " @scope: " + scope);
			t = _New(type, scope);
			singletonInstances[type].Add(scope, t);
		}
		t = (ScriptableObject)singletonInstances[type][scope];
		return t;
	}

	public static void Release () {
		instance.Dispose();
	}

	public void Dispose () {
		GC.SuppressFinalize(this);
	}

	public T _New<T> () where T : ScriptableObject {
		return ScriptableObject.CreateInstance<T>();
	}

	public ScriptableObject _New (Type type, string scope = "") {
		string typeString = type.ToString();
		BindMap bindmap = null;
		ScriptableObject returnSO;
		if(System.Array.Exists<BindMap>(injectMap.bind, (BindMap b) => b.bind.Equals(typeString) && b.scope.Equals(scope)))
			bindmap = System.Array.Find<BindMap>(injectMap.bind, (BindMap b) => b.bind.Equals(typeString) && b.scope.Equals(scope));
		if (bindmap != null) 
		{
			Log(string.Format("bind {0} to {1}", bindmap.bind, bindmap.to));

			returnSO = ScriptableObject.CreateInstance(bindmap.to);

			//Inject into member
			_Inject(returnSO);
			return returnSO;
		} else {
			logger.LogErrorFormat("Binding map doesn't set corresponding name to {0}, scope {1}.", typeString, scope);
			return null;
			// throw new Exception("Binding map doesn't set corresponding name to " + typeString);
		}
	}

#region DependencyInjection
	public static void Inject (object target) {
		instance._Inject(target);
	}
	public void _Inject (object target) {
		string message = "";
		Log("!!Injecting... " + target);
		try 
		{
			foreach(FieldInfo fInfo in target.GetType().GetFields(BindingFlags.Public |
															   BindingFlags.NonPublic |
															   BindingFlags.Instance )){
				foreach(InjectAttribute aInfo in fInfo.GetCustomAttributes(typeof(InjectAttribute), false)){
					message = "!!Inject: " + target.GetType() + ", " + target.GetType().BaseType + ", " +
						fInfo.Name + ": " + fInfo.FieldType;
					if(aInfo.singleton){
						if(fInfo.FieldType.IsPrimitive || fInfo.FieldType == typeof(string)) {
							continue;
						} else {
							Log(target.GetType() + ", FieldType: " + fInfo.FieldType + ", scope: " + aInfo.scope);
							fInfo.SetValue(target, _Get(fInfo.FieldType, aInfo.scope));
						}
					} else {
						if(fInfo.FieldType.IsPrimitive || fInfo.FieldType == typeof(string)) {
							InjectPrimitive(fInfo, target);
						} 
						else if (fInfo.FieldType.IsArray) {
							InjectPrimitiveArray(fInfo, target);
						}else {
							if(fInfo.GetValue(target) == null){
								fInfo.SetValue(target, _New(fInfo.FieldType));
							}
						}
					}
				}
			}
		} catch (System.Exception e) 
		{
				LogError("Inject Fail msg: " + message + "Exception: " + e.ToString());
		} finally 
		{
			////////////////////
			// Self injection //
			////////////////////
			if(target.GetType() == typeof(Injector)){
				// Inject(logger);
				logger.Context = this.GetType().ToString();
			}

			Log("Inject done msg: " + target);
		}
	}

	private void InjectPrimitiveArray (FieldInfo fInfo, object target) {
		try{
			// TypeCode will get Object, and FieldType cannot used in switch directly, so we use string.
			switch(fInfo.FieldType.ToString()){
				case "System.Boolean[]":
					fInfo.SetValue(target, _GetArray<bool>(target.GetType(), fInfo.Name));
				break;
				case "System.Byte[]":
					fInfo.SetValue(target, _GetArray<byte>(target.GetType(), fInfo.Name));
				break;
				case "System.Char[]":
					fInfo.SetValue(target, _GetArray<char>(target.GetType(), fInfo.Name));
				break;
				case "System.Decimal[]":
					fInfo.SetValue(target, _GetArray<decimal>(target.GetType(), fInfo.Name));
				break;
				case "System.Double[]":
					fInfo.SetValue(target, _GetArray<double>(target.GetType(), fInfo.Name));
				break;
				case "System.Int16[]":
					fInfo.SetValue(target, _GetArray<short>(target.GetType(), fInfo.Name));
				break;
				case "System.Int32[]":
					fInfo.SetValue(target, _GetArray<int>(target.GetType(), fInfo.Name));
				break;
				case "System.Int64[]":
					fInfo.SetValue(target, _GetArray<long>(target.GetType(), fInfo.Name));
				break;
				case "System.SByte[]":
					fInfo.SetValue(target, _GetArray<sbyte>(target.GetType(), fInfo.Name));
				break;
				case "System.Single[]":
					fInfo.SetValue(target, _GetArray<float>(target.GetType(), fInfo.Name));
				break;
				case "System.String[]":
					fInfo.SetValue(target, _GetArray<string>(target.GetType(), fInfo.Name));
				break;
				case "System.UInt16[]":
					fInfo.SetValue(target, _GetArray<ushort>(target.GetType(), fInfo.Name));
				break;
				case "System.UInt32[]":
					fInfo.SetValue(target, _GetArray<uint>(target.GetType(), fInfo.Name));
				break;
				case "System.UInt64[]":
					fInfo.SetValue(target, _GetArray<ulong>(target.GetType(), fInfo.Name));
				break;
				default:
					logger.LogWarningFormat("TypeCode:{0}, IsArray:{1}", fInfo.FieldType, fInfo.FieldType.IsArray);
				break;
			}
		} catch (PrimitiveMapNotDeclareException e) {
			logger.LogFormat("Array value of {0} not declared.\n{1}", fInfo.Name, e.ToString());
		} catch (AmbiguousMatchException ame) {
			logger.LogWarning("Inject primitive fail \n" + ame.ToString());
		}
	}

	private void InjectPrimitive (FieldInfo fInfo, object target) {
		try{
			switch(Type.GetTypeCode(fInfo.FieldType)){
				case TypeCode.Boolean:
					fInfo.SetValue(target, _Get<bool>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Byte:
					fInfo.SetValue(target, _Get<byte>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Char:
					fInfo.SetValue(target, _Get<char>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Decimal:
					fInfo.SetValue(target, _Get<decimal>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Double:
					fInfo.SetValue(target, _Get<double>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Int16:
					fInfo.SetValue(target, _Get<short>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Int32:
					fInfo.SetValue(target, _Get<int>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Int64:
					fInfo.SetValue(target, _Get<long>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.SByte:
					fInfo.SetValue(target, _Get<sbyte>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.Single:
					fInfo.SetValue(target, _Get<float>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.String:
					fInfo.SetValue(target, _Get<string>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.UInt16:
					fInfo.SetValue(target, _Get<ushort>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.UInt32:
					fInfo.SetValue(target, _Get<uint>(target.GetType(), fInfo.Name));
				break;
				case TypeCode.UInt64:
					fInfo.SetValue(target, _Get<ulong>(target.GetType(), fInfo.Name));
				break;
				default:
					logger.LogWarning("Unsupported type of primitive injection");
				break;
			}
		} catch (PrimitiveMapNotDeclareException e) {
			logger.LogFormat("Value of {0} not declared.\n{1}", fInfo.Name, e.ToString());
		} catch (AmbiguousMatchException ame) {
			logger.LogWarning("Inject primitive fail \n" + ame.ToString());
		}
	}
#endregion			
	void Log (string msg) {
		if(logger != null){
			logger.Log(msg);
		} else {
			// Debug.Log(msg);
		}
	}

	void LogWarning (string msg) {
		if(logger != null){
			logger.LogWarning(msg);
		} else {
			// Debug.LogWarning(msg);
		}
	}

	void LogError (string msg) {
		if(logger != null){
			logger.LogError(msg);
		} else {
			// Debug.LogError(msg);
		}
	}
}