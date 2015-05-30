using System;

namespace PowIoC 
{
	/// <summary>
	/// This attribute use to note the field that needs to be injected
	/// </summary>
	/// <example>
	/// <code>
	///
	///		public abstract class YourClass
	///		{ 
	///			[Inject] 
	///			public YourSingletonClass mySingletonClass; 
	///			[Inject(singleton = false)]
	///			public YourNonSingleClass myNonsingletonClass;
	///			[Inject(false)]
	///			public YourNonSingleClass myNonsingletonClassAlternate;
	///			[Inject(false, "OtherPurpose")]
	///			public YourNonSingleClass myNonsingletonClassAnother;
	///			[Inject(false)]
	///			protected int myInt;
	///			[Inject(false)]
	///			protected float[] myFloatAry;
	///
	///
	/// </code>
	/// </example>
	//TODO
	// [Inject(singleton = false)]
	// public AbstractLoginInfo[] loginInfoNewAry;
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property)]
	public class InjectAttribute : Attribute {
		public bool singleton = true;
		public string scope = "";

		public InjectAttribute () {

		}

		public InjectAttribute (bool singleton) {
			this.singleton = singleton;
		}

		public InjectAttribute (string scope) {
			this.scope = scope;
		}

		public InjectAttribute (bool singleton, string scope) {
			this.singleton = singleton;
			this.scope = scope;
		}
	}
}
