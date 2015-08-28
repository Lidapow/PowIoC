#PowIoC

An Inversion of Control (Dependency Injection) for Unity3D.
This library utilize ScriptableObject of Unity3D to descript mapping relation.

The main purpose is no need to recompile source code, when there just have some dependency changed.

Following snippet is the dependency settings, which stored in a asset file.
You can edit it in Inspector window, even in a text editor.

Due to the binding data isn't source code, so the data also can be changed, even your application was built.

![Inspector window of InjectMap](https://raw.githubusercontent.com/Lidapow/PowIoC/master/res/img/InspectorOfInjectMap.png)
```YAML
  bind:
  - bind: SomeData
    to: SomeData
    scope: 
    note: 
  - bind: ICommand
    to: CommandA
    scope: PurposeA
    note: 
  - bind: ICommand
    to: CommandB
    scope: PurposeB
    note: 
  - bind: ILogger
    to: UnityLogger
    scope: 
    note: 
```

Following code shows how to mark fields which needs inject, and can pass parameter to set the scope and non-singleton.

```C#
using UnityEngine;
using System.Collections;
using PowIoC;

public class MyComponent : MonoBehaviour {
	[Inject]
	SomeData data; 

	[Inject("PurposeA")]
	ICommand cmdA;
	[Inject("PurposeB")]
	ICommand cmdB;

	[Inject(false)]
	ILogger logger;

	void Awake () {
		Injector.Inject(this);
	}
}
```

For setting value changing in delopy version, please investigate the `Settings.xml` which placed in Unity project folder (what contains Assets).

Before Injector.Inject called, the value of InjectMap as following.
```YAML
  bind:
  - bind: ExampleChangedFromXML
    to: Oringinal
    scope: 
    note: 
  primitive:
  - fieldPath: ExampleChangedFromXML
    fieldValue: 100
    note: 
  primitiveArray:
  - fieldPath: ExampleChangedFromXML
    fieldValue:
    - One
    - Two
    - Three
    note: Ignore class
```
After Injector.Inject called, the value will changed as following.
```YAML
  bind:
  - bind: ExampleChangedFromXML
    to: Changed
    scope: 
    note: 
  primitive:
  - fieldPath: ExampleChangedFromXML
    fieldValue: 900
    note: 
  primitiveArray:
  - fieldPath: ExampleChangedFromXML
    fieldValue:
    - Alpha
    - Beta
    - Charlie
    note: Ignore class
```

