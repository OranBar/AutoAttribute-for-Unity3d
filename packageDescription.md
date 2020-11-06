This package provides [Auto], [AutoParent] and [AutoChildren] attributes. \n
\n
They allow to automatically get/fetch references of components at the very start of the application. \n
\n
Using auto, getting your components is as simple as preprending the appropriate [Auto*] attribute to your variable declarations. \n
\n
Auto can be used with all access modifiers (private, protected, public), and all types (Unity components and custom monobehaviours). \n
\n
[AutoParent] and [AutoChildren] can be used either to get a single component, or get all the components as an array. \n
\n
Here are a couple of examples: \n
\n
\t[Auto] public Rigidbody myRb; \n
\n
\t[AutoParent] protected Collider[] myParentsColliders; \n
\n
\t[AutoChildren] private CustomScript scriptInOneOfChildren;\n