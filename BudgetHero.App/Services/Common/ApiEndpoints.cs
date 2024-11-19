namespace BudgetHero.App.Services.Common
{
    public static class ApiEndpoints
    {
        //public static readonly string BaseAddress = DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5070" : "http://localhost:5070";

        public static readonly string BaseAddress = "https://npdxm779-7201.euw.devtunnels.ms";

        public static readonly string Login = "/login";
        public static readonly string Register = "/register";
        public static readonly string Refresh = "/refresh";
        public static readonly string ConfirmEmail = "/confirmEmail";
        public static readonly string ResendConfirmationEmail = "/resendConfirmationEmail";
        public static readonly string ForgotPassword = "/forgotPassword";
        public static readonly string ResetPassword = "/resetPassword";
        public static readonly string Manage2fa = "/manage/2fa";
        public static readonly string ManageInfo = "/manage/info";

        public static readonly string Api = "/api";

        public static readonly string User = "/User";
        public static readonly string Address = "/Address";
        public static readonly string Budget = "/Budget";
        public static readonly string Transaction = "/Transaction";

        public static readonly string TransactionCategory = "/TransactionCategory";

        public static readonly string DateRange = "/DateRange";
        public static readonly string Filtered = "/Filtered";
        public static readonly string TopAmount = "/TopAmount";
        public static readonly string TopCount = "/TopCount";
    }
}
