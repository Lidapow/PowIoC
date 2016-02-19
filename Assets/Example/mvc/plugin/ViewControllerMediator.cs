using UnityEngine;
using System.Collections;
using PowIoC;

public class ViewControllerMediator : ScriptableObject, ISetup, IPlugin {
	[Inject(false)]
	ILogger logger;

	public void Setup () {
		logger.Context = this.GetType().ToString();
	}

	public void PreInject (object target, object field) {
		// if(logger != null) logger.Log("");
		IViewContainer container = field as IViewContainer;
		IView obj = target as IView;
		if (container != null && obj != null) {
			container.Register(obj);
		}
	}

	public void PostInject (object target, object field) {
		// if(logger != null) logger.Log("");
	}


} 