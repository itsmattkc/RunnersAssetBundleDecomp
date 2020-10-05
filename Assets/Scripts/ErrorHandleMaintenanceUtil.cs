using DataTable;
using System;

public class ErrorHandleMaintenanceUtil
{
	public static string GetMaintenancePageURL()
	{
		return InformationDataTable.GetUrl(InformationDataTable.Type.MAINTENANCE_PAGE);
	}

	public static bool IsExistMaintenancePage()
	{
		string maintenancePageURL = ErrorHandleMaintenanceUtil.GetMaintenancePageURL();
		return !string.IsNullOrEmpty(maintenancePageURL);
	}
}
