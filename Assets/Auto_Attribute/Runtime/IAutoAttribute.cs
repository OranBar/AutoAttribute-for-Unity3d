using System;
using UnityEngine;

public interface IAutoAttribute {
	bool Execute(MonoBehaviour mb, Type varType, Action<MonoBehaviour, object> SetVariableType);


}
