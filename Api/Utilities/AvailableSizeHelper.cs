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

        public static string[] GetAvaliableSizesAsStrings(AvaliableSizes avaliableSizes)
        {
            List<string> strs = [];

            foreach (AvaliableSizes size in Enum.GetValues<AvaliableSizes>())
            {
                if (size == AvaliableSizes.None || size == AvaliableSizes.All)  // skip these values
                    continue;

                if (avaliableSizes.HasFlag(size))
                    strs.Add(size.ToString());
            }

            return [.. strs];
        }
    }


}
