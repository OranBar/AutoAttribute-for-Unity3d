# AutoAttribute_unity3d

# Auto Attributes

This package provides 3 attributes:
[Auto], [AutoParent], [AutoChildren]

Autos are field/property attributes that allow to automatically get/fetch components' references at the very start of the application. 
This solution is presented as equivalent of having an Awake function containing GetComponent calls, without having to worry about ordering or disabled gameObjects.

It runs before all other Awakes (using script execution ordering), so when Awake starts, one can assume that no variable with Auto will throw a null exception.
In case a component couldn't be found, Auto will provide with descriptive logging.

[AutoParent] and [AutoChildren] can also be used on Array variables and Lists, and respectively has the same behaviour you would expect from a GetComponentsInParents<T>() or GetComponentsInChildren<T>() call. 
  
Auto also works on inactive objects.

Auto works by hooking into the awake function of a Manager monobehaviour script, which is automatically spawned in the scene whenever it is saved.

# Instantiation

In case of instantiated objects, please use the MonoBehaviour extension method "this.Instantiate_And_AutoAssignVariables()" to make sure Auto does the referencing before anyone else starts using that class


# Requisites

All scripts using Auto must have their script execution order delay < 990.

# Installation

As soon as the package is install, you can already make use of the Auto Attributes!

