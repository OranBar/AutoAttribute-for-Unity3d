using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public interface IAutoAttribute {
	bool Execute(MonoBehaviour mb, Type varType, Action<MonoBehaviour, object> SetVariableType);


}
