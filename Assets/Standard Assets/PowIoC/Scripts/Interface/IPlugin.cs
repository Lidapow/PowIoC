namespace PowIoC {
	public interface IPlugin {
		void PreInject (object host, object client);
		void PostInject (object host, object client);
	}
}