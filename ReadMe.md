#PowIoC

A Inversion of Control (Dependency Injection) for Unity3D.
This library utilize ScriptableObject of Unity3D to descript mapping relation.

The main purpose is no need to recompile source code, when there just have some dependency changed.


```
using PowIoC;
void Awake () {
	Injector.Inject(this);
}
```