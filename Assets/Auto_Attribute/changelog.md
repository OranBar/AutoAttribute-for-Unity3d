changelog

v3.0.0 
- Project ufficially becomes a package
- Updated Auto to Unity 2019.4
- Added MonoBehaviour extension methods to instantiate and object and immediately auto assign its variables. The following code can run anywhere in a MonoBehaviour => this.Instantiate_And_AutoAssignAuto([...])
	Reasoning: Since Auto works on Awake, it will not be called before the current method is completed. This means that instantiated objects with auto will not be able to assign the variables in time, and methods called from the method that instantiated the object will result in null exceptions. Using the extension method, auto will immediately reference auto variables before returning the instantiated object. 
- Removed use of dynamic word that would cause problems with Unity's compiler
- Improved logging
- Improved project structure
