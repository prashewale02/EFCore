
namespace EFCore.InventoryModels
{
    public class InventoryModelsConstants
    {
        // Item
        public const int MAX_DESCRIPTION_LENGTH = 250;
        public const int MAX_NAME_LENGTH = 100;
        public const int MAX_NOTES_LENGTH = 2000;
        public const int MAX_USERID_LENGTH = 50;
        public const int MIN_QUANTITY = 0;
        public const int MAX_QUANTITY = 1000;
        public const double MIN_PRICE = 0.0;
        public const double MAX_PRICE = 25000.0;
        
        // Color
        public const int MAX_COLORVALUE_LENGTH = 25;
        public const int MAX_COLORNAME_LENGTH = 25;

        // Player
        public const int MAX_PLAYERNAME_LENGTH = 50;
        public const int MAX_PLAYERDESCRIPTION_LENGTH = 500;

        // Genre
        public const int MAX_GENRENAME_LENGTH = 50;
    }
}
