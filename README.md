# Hellmo
Elmo welcomes you to hell! 
This is a language designed to save key strokes (with the possible cost of your sanity).

### Support
Due to the fact that this is a prototype, this project is written in C#, developedd with .Net 6 (soon to be updated with .Net 7 on November 2022), All of the Desktop Operating Systems (and other OSs that have .Net Support) are supported automatically (compilation script coming soon).
 - [x] Linux
 - [x] Windows
 - [x] MacOS

 ### Documentation <br>
 `0x0b` Exit <br>
 `0x01` mov up <br>
 `0x02` mov down <br>
 `0x03` In current script, Jump to this instruction <br>
 `0x04` In stack, Jump to this address <br>
 `0x05` increment current address by 1 <br>
 `0x06` decrement current address by 1 <br>
 `0x07` Set this address to this value <br>
 `0x08` Print (To be further developped) <br>
 `0x09` if statement

 #### If statement
If the if statement is true, it will jump to the targeted adddress (in this case, if (Address 13 is equal to 14, jump to address 15)).
Otherwise, it will skip 3 addresses (in the example below, right after `0xF`)

`0x05 0xD 0x0E 0xF`:<br>
 "If"&nbsp;&nbsp;"this addr"&nbsp;&nbsp;"is this value"&nbsp;&nbsp;"Jump to this point"<br>
 0x05&nbsp;&nbsp;&nbsp;&nbsp;0xD&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0xE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0xF<br>
  if&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[13]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[14]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[15] <br>

 #### Jump to and alter address
This feature was made so you dont have to keep spamming `0x05` or  `0x06` contstantly, flooding the device with instructions, making it slow and highly inefficient. 

__For instance:__
```
0x08 "[Hello World!]"
0x08 "This is a print test script!"
0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05
0x01 
0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 0x05 
0x08 stack
0x00
```

Instead, you have:
```
0x08 "[Hello World!]"
0x08 "This is a print test script!"
0x07 0x00 0xA
0x01 
0x07 0x01 0x18
0x08 stack
0x00
```

<img src="image.webp" alt="drawing" width="200"/>

<ins> Do note that this is a prototype of the C++ version (which is the actual version) of this project/repo. The actual version will come out at its initial release </ins>