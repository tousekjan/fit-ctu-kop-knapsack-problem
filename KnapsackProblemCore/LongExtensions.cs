namespace KnapsackProblemCore
{
    public static class LongExtensions
    {
        /// <summary>
		/// Returns true if the bit on bitIndex position is set to 1, false otherwise
		/// </summary>
        public static bool IsBitSet(this long number, int bitIndex)
        {
            return ((number >> bitIndex) & 1) != 0;
        }

        /// <summary>
		/// Sets the bitIndex bit to value
		/// </summary>
        public static void SetBit(this long number, int bitIndex)
        {
            number |= (uint)1 << bitIndex;
        }
    }
}
