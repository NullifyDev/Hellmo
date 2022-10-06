# Hellmo
Elmo welcomes you to hell! 
This is where you physically write actual machine code using hex.

### Support
Due to the fact that this is C#, developedd with .Net 6, All of the Desktop Operating Systems are supported automatically (compiler script coming soon).
 - [x] Linux 
 - [x] Windows
 - [x] MacOS

 ### Documentation <br>
 `0x0b` Exit <br>
 `0x01` mov up <br>
 `0x02` mov down <br>
 `0x03` In current script, Jump to this instruction <br>
 `0x04` In stack, Jump to this adress <br>
 `0x05` increment current address by 1 <br>
 `0x06` decrement current address by 1 <br>
 `0x07` Set this address to this value <br>
 `0x08` if statement

 #### If statement
If the if statement is true, it will jump to the targeted adddress (in this case, if (Address 13 is equal to 14, jump to address 15)).
Otherwise, it will skip 3 addresses (in the example below, right after `0xF`)

`0x05 0xD 0x0E 0xF`:<br>
 "If"&nbsp;&nbsp;"this addr"&nbsp;&nbsp;"is this value"&nbsp;&nbsp;"Jump to this point"<br>
 0x05&nbsp;&nbsp;&nbsp;&nbsp;0xD&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0xE&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;0xF<br>
  if&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[13]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[14]&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;[15] <br>

<img src="image.webp" alt="drawing" width="200"/>

<ins> Do note that this is a prototype of the C++ version (which is the actual version) of this project/repo. The actual version will come out at its initial release </ins>
