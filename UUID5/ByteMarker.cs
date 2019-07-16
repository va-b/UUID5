using System;

namespace UUID5
{
    internal static class ByteMarker
    {
        private const int VariantIndexPosition = 8;
        private const int VariantMask = 0x3f;
        private const int VariantBits = 0x80;
        private const int VersionIndexPosition = 7;
        private const int VersionMask = 0x0f;
        private const int Version = 0x50; 
        
        public static byte[] AddMarkers(this byte[] array)
        {
            array[VariantIndexPosition] &= VariantMask;
            array[VariantIndexPosition] |= VariantBits;
            //
            array[VersionIndexPosition] &= VersionMask;
            array[VersionIndexPosition] |= Version;
            //
            return array;
        }

        public static byte[] TrimTo16Bytes(this byte[] array)
        {
            var result = new byte[16];
            Array.Copy(array, result, 16);
            return result;
        }
    }
}
