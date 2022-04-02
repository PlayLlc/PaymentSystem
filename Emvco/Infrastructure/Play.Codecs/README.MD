# Play LLC

## Module Description

>This module transforms card related data into a format that can be understood by Payment Processors and Payment Gateways who utilize the ISO/IEC 8583 standard. 

>Payment Processors and Gateways who do utilize this standard will have a slightly different implementation, but this module can be used as a baseline to simplify the integration for Processors and Gateways who utilze this standard


## Module Specifications


> ### Alphabetic
>>Alphabetic data elements contain a single character per byte. The permitted characters are alphabetic only (a to z and A to Z, upper and lower case).


> ### Alpha-Numeric
>>Alphanumeric data elements contain a single character per byte. 
The permitted characters are alphabetic (a to z and A to Z, upper and lower case) and numeric (0 to 9).


> ### Alpha-Numeric-Special
>>Alphanumeric Special data elements contain a single character per byte. The permitted characters and their coding are shown in the Common Character Set table in Annex B. 

>>There is one exception: The permitted characters for Application Preferred Name are the non-control characters defined in the ISO/IEC 8859 part designated in the Issuer Code Table Index associated with the Application Preferred Name


> ### Alpha-Special
>>These data elements contain a single character per byte. The permitted characters are alphabetic (a to z and A to Z, upper and lower case) as well as any Special Characters


> ### Binary 
>>These data elements consist of either unsigned binary numbers or bit combinations that are defined elsewhere in the specification. 

>>Binary example: The Application Transaction Counter (ATC) is defined as “b” with a length of two bytes. An ATC value of 19 is stored as Hex '00 13'. 

>>Bit combination example: Processing Options Data Object List (PDOL) is defined as “b” with the format shown in Book 3, section 5.4.

> ### Compressed Numeric 
>>Compressed numeric data elements consist of two numeric digits (having values in the range Hex '0'–'9') per byte. 
These data elements are left justified and padded with trailing hexadecimal 'F's.


> ### Hexadecimal
>>In most current use cases, the letters A–F or a–f represent the values 10–15, while the numerals 0–9 are used to represent their decimal values.


> ### Numeric
>>Numeric data elements consist of two numeric digits (having values in the range Hex '0'–'9') per byte. 
These digits are right justified and padded with leading hexadecimal zeroes. 
Other specifications sometimes refer to this data format as Binary Coded Decimal (“BCD”) or unsigned packed.

>>Example: Amount, Authorised (Numeric) is defined as “n 12” with a length of six bytes. 
A value of 12345 is stored in Amount, Authorised (Numeric) as Hex '00 00 00 01 23 45'.

> ### Numeric Special
>>Numeric data elements consist of one character per byte. These characters can be Numeric or Special encoded characters


> ### Signed Integer 
>>These data elements consist of signed binary numbers. [Two's Compliment](https://en.wikipedia.org/wiki/Two%27s_complement) is used to encode and decode these data objects
 

> ### Signed Numeric
>>Signed Numeric consists of a two characters per byte. 
These characters consist of a leading indicator (0xC, or 0xD) to distinguish between positive and negative values, followed by Hexadecimal characters [0x0 - 0x9]. 
The leading indicators are either a '0xC' or a '0xD'. A leading '0xC' indicates a positive value and a leading '0xD' indicates a negative value.
   

> ### Special
>>Special Characters (32–47 / 58–64 / 91–96 / 123–126): Special characters include all printable characters that are neither letters nor numbers. These include punctuation or technical, mathematical characters.
  

> ### Unsigned Integer 
>>These data elements consist of unsigned binary numbers. These Unsigned Integers can be of an unspecified size and don't have to conform to native type encodings, like Uint64, etc











 














