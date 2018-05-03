# AutoAttribute_unity3d

# Auto Attributes

This package provides 3 attributes:
[Auto], [AutoParent], [AutoChildren]

Autos are field/property attributes that allow to automatically get/fetch components' references at the very start of the application. 
This solution is presented as equivalent of having an Awake function containing GetComponent calls, without having to worry about ordering or disabled gameObjects.

It runs before all other Awakes (using script execution ordering), so when Awake starts, one can assume that no variable with Auto will throw a null exception.
In case a component couldn't be found, Auto will provide with descriptive logging.

[AutoParent] and [AutoChildren] can also be used on Array variables and Lists, and respectively has the same behaviour you would expect from a GetComponentsInParents<T>()/GetComponentsInChildren<T>() call. 
  
Auto also works on inactive objects.

# Instantiation

In case of instantiated objects, adding the component AutoReferencerOnInstantiation will make sure that Auto variables are referenced as soon as possible. 
Again, check to make sure the script's execution order is higher priority than any other script on the gameObject.


# Installation

Unpack the latest unityPackage Auto release, and start using the attributes straight away. 

