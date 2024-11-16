namespace TheWarpZone.Common.Constraints
{
    public static class MovieConstraints
    {
        public const int TitleMaxLength = 200;
        public const int TitleMinLength = 5;
        public const int DescriptionMaxLength = 1000;
        public const int DirectorMaxLength = 200;
        public const int DirectorMinLength = 2;
    }

    public static class TVShowConstraints
    {
        public const int TitleMaxLength = 200;
        public const int TitleMinLength = 5;
        public const int DescriptionMaxLength = 1000;
    }
    public static class ReviewConstraints
    {
        public const int CommentMaxLength = 1000;       
        public const int CommentMinLength = 10;         
    }

    public static class CastMemberConstraints
    {
        public const int NameMaxLength = 100;           
        public const int NameMinLength = 2;             
        public const int RoleMaxLength = 100;           
        public const int RoleMinLength = 2;             
    }

    public static class EpisodeConstraints
    {
        public const int EpisodeTitleMaxLength = 200;          
        public const int EpisodeTitleMinLength = 5;            
        public const int EpisodeDescriptionMaxLength = 500;    
        public const int EpisodeDescriptionMinLength = 20;     
    }

    public static class SeasonConstraints
    {
        public const int SeasonNumberMin = 1;   
    }

    public static class RatingConstraints
    {
        public const int RatingMin = 1;        
        public const int RatingMax = 5;        
    }


    public static class TagConstraints
    {
        public const int NameMaxLength = 50;             
        public const int NameMinLength = 2;             
    }
}
