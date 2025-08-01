using Api.Constants;

namespace Api.Utilities
{
    public static class AvailableSizeHelper
    {
        public static AvaliableSizes GetAvaliableSizes(string[] sizes)
        {
            return sizes.Aggregate(AvaliableSizes.None, (acc, next) => acc | GetAvaliableSizes(next));
        }

        public static AvaliableSizes GetAvaliableSizes(string size)
        { 
            return size.ToUpper() switch
            {
                "XS" => AvaliableSizes.XS,
                "S" => AvaliableSizes.S,
                "M" => AvaliableSizes.M,
                "L" => AvaliableSizes.L,
                "XL" => AvaliableSizes.XL,
                "XXL" => AvaliableSizes.XXL,
                "XXXL" => AvaliableSizes.XXXL,
                _ => AvaliableSizes.None
            };
        }
    }


}
