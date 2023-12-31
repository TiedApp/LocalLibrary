namespace GlobalShared
{
    public static class LibVariables
    {
#if DEBUG
        public static string TiedAppUrl { get; } = "https://localhost:5114/";
#else
        public static string TiedAppUrl { get; } = "https://tiedapp.com/";
#endif
        public static string RequestCodeURL { get; } = $"{TiedAppUrl}LocalLib/RequestCode";
        public static string ValidateCodeURL { get; } = $"{TiedAppUrl}LocalLib/ValidateCode";
        public static string DeleteFailedPostFileURL { get; } = $"{TiedAppUrl}LocalLib/DeleteFailedPostFile";
        public static string CompanyLibrary { get; } = "CL_Main"; //CAN BE MODIFY
        public static string ActionRequest { get; } = "CL_AR"; //CAN BE MODIFY
        public static string InternalCom { get; } = "CL_COM"; //CAN BE MODIFY
    }
}
