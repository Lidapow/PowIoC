namespace PowIoC 
{
	public interface IParsable {
		T Parse<T> (string rawData) where T : new ();
		string ToRawString (object data);
	} 
}