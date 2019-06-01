public class Controllers {

	public static bool Run(BaseController baseController) {
		return baseController != null && baseController.Execute();
	}
}
