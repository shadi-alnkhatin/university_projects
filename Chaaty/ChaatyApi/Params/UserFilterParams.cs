namespace ChaatyApi.Params
{
    public class UserFilterParams:PaginationParams
    {
        public string? LanguageName { get; set; }
        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string? Country { get; set;}
    }
}
