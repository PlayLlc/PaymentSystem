# Play LLC

## Module Description

>This module transforms card related data into a format that can be understood by Payment Processors and Payment Gateways who utilize the ISO/IEC 8583 standard. 

>Payment Processors and Gateways who do utilize this standard will have a slightly different implementation, but this module can be used as a baseline to simplify the integration for Processors and Gateways who utilze this standard


## Module Specifications


> ### Alphabetic Codec
>>Alphabetic data elements contain a single character per byte. The permitted characters are alphabetic only (a to z and A to Z, upper and lower case).

> ### Compressed Numeric 
>>Compressed numeric data elements consist of two numeric digits (having values in the range Hex '0'–'9') per byte. 
>>These data elements are left justified and padded with trailing hexadecimal 'F's.

>>Example: The Application Primary Account Number (PAN) is defined as “cn” with a length of up to ten bytes. 
>>A value of 1234567890123 may be stored in the Application PAN as Hex '12 34 56 78 90 12 3F FF' with a length of 8.

> ### AlphaNumeric Codec
>>Alphanumeric data elements contain a single character per byte. 
>>The permitted characters are alphabetic (a to z and A to Z, upper and lower case) and numeric (0 to 9).


> ### AlphaSpecial Codec
>>These data elements contain a single character per byte. The permitted characters are alphabetic (a to z and A to Z, upper and lower case) as well as any .








