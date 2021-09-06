using System;

namespace GCommon.Primitives.Helpers
{
    public class HexReader
    {
        private const int HeaderLength = 4;
        private const int ArrayHeaderLength = 20;
        private const int ArrayPaddingLength = 8;
        private const int ArraySizeLength = 4;
        private const int ElementSizeLength = 8;
        private readonly Hex _input;

        public HexReader(string input)
        {
            if (!input.StartsWith("0x"))
                throw new ArgumentException();
            
            _input = new Hex(input);
        }

        public Hex Header => _input.Head(HeaderLength);
        public Hex ArrayHeader => IsArray ? _input.Take(HeaderLength, ArrayHeaderLength) : Hex.Empty;
        public Hex Data => IsArray ? _input.DropHead(HeaderLength + ArrayHeaderLength) : _input.DropHead(HeaderLength);
        public bool IsArray => _input[2].ToBool();
        public int TypeId => _input[3].ToInt();
        public int ArrayLength => IsArray 
            ? ArrayHeader.Take(ArrayPaddingLength, ArraySizeLength).Reverse().ToInt() 
            : -1;
        
        /// <summary>
        /// Size of the array elements. Since the raw value is in bytes we need to multiple by 2.
        /// </summary>
        public int ElementSize => IsArray
                ? ArrayHeader.Take(ArrayPaddingLength + ArraySizeLength, ElementSizeLength).Reverse().ToInt() * 2
                : -1;
    }
}