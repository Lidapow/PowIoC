namespace PowIoC {
	/// <summary>
	/// Injector will call this after instantiated.
	/// </summary>
	public interface ISetup {
		void Setup();
	}
}