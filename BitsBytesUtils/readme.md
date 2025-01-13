# BitsBytesUtils

BitsBytesUtils is a small project (full generic) that can be used to convert all numeric data types into their bytes or bits and convert them back to the original data type. In addition, further information can be queried, such as BitLength, LeadingZero, TwosComplement, PowTwo etc. Feel free to try out the possibilities.

The following tests are performed:

- #### TestBitLength:
  Returns the exact bit length that a number has in the binary representation.
  
- #### TestLeadingZerosCount:
  Returns the exact number of leading zeros.
  
- #### TestTwosComplementBigInteger:
  Returns the TwosComplement for the BigInteger. Is mainly used if the range is not known.
  
- #### TestBits:
  Returns the binary representation of a number as a bool array. There is a choice between big and little endian.
  
- #### TestBitStringBytes:
  Returns the BitString or the BitBytes as a representation of the number.

- #### TestBytes:
  Jede beliebige Zahl kann in eine Array of byte umgewandelt werden.

I will add more tests at a later time.
