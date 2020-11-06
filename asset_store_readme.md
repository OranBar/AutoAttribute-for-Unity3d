# AutoAttribute_unity3d<br>
<br>
<strong># Auto Attributes<br></strong>
<br>
This package provides 3 attributes:<br>
<em>[Auto]</em>, <em>[AutoParent]</em>, <em>[AutoChildren]</em><br>
<br>
Autos are field/property attributes that allow to automatically get/fetch components' references at the very start of the application. <br>
This solution is presented as equivalent of having an Awake function containing GetComponent calls, without having to worry about ordering or disabled gameObjects.<br>
<br>
It runs before all other Awakes (using script execution ordering), so when Awake starts, one can assume that no variable with Auto will throw a null exception.<br>
In case a component couldn't be found, Auto will provide with descriptive logging.<br>
<br>
[AutoParent] and [AutoChildren] can also be used on Array variables and Lists, and respectively has the same behaviour you would expect from a <em>T[] GetComponentsInParents()</em> or <em>T[] GetComponentsInChildren()</em> call. 
  <br>
Auto also works on inactive objects.<br>
<br>
Auto works by hooking into the awake function of a Manager monobehaviour script, which is automatically spawned in the scene whenever it is saved.<br>
<br>
<strong># Instantiation<br></strong>
<br>
In case of instantiated objects, please use the MonoBehaviour extension method <em>this.Instantiate_And_AutoAssignVariables()</em> to make sure Auto does the referencing before anyone else starts using that class<br>
<br>
<br>
<strong># Requisites<br></strong>
<br>
All scripts using Auto must have their script execution order delay < 990.<br>
<br>
<strong># Installation<br></strong>
<br>
As soon as the package is install, you can already make use of the Auto Attributes!<br>