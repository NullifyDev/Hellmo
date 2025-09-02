# THIS IS ASS! I AM ALREADY REWRITING THIS AS OF 2ND AUG 2025!!

# Hellmo
Elmo welcomes you to hell!<br>
This is a low-to-high-end languge, designed to be flexible.<br>
Much like America, we like to save keystrokes by unnecessarily removing all important things like `+`. And instead of typing `System.Console.WriteLine("Hello World!");`, you have `0x08 "Hello World!"`<br>
Geting use to this may be hard. But once you get the hang of it, it might get easy to use (depending on who you ask).

### Support
Due to the fact that this is a prototype, this project is written in C#, developedd with .Net 6 (soon to be updated with .Net 7 on November 2022), All of the Desktop Operating Systems (and other OSs that have .Net Support) are supported automatically (compilation script coming soon).
 - [x] Linux
 - [x] Windows
 - [x] MacOS

 ### Documentation <br>
 `0x00` return <br>
 `0x01` mov up <br>
 `0x02` mov down <br>
 `0x03` In current script, Jump to this instruction <br>
 `0x04` In stack, Jump to this address <br>
 `0x05` increment current address by 1 <br>
 `0x06` decrement current address by 1 <br>
 `0x07` Set this address to this value <br>
 `0x08` Print (To be further developped) <br>
 `0x09` if statement
 `0x0A` 
 `0x0B` Exit <Hex> (Default hex value/argument: 0x00 (0))
 `0x0C` 
 `0x0D` 
 `0x0E` 
 `0x0F` Make a function with the id of `<id>` with arguments of `[ ..., ... ]`

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

