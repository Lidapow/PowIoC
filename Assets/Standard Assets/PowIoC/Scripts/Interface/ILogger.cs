// namespace TinyIoC 
// {
	public interface ILogger {
		string Context {get; set;}
		void Log (object message);
		void LogWarning (object message);
		void LogError (object message);
		void LogFormat (string format, params object[] messages);
		void LogWarningFormat (string format, params object[] messages);
		void LogErrorFormat (string format, params object[] messages);
	}
// }