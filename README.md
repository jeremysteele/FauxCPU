# FauxCPU

A fake emulator for a fake CPU created in my fake brain

## What is this thing?

FauxCPU is a fake 8-bit little-endian CPU loosely based on the 6502 but not really. I decided to build this as a learning experiment.... plus I got bored and needed something fun to do. 

It features 3 8-bit general purpose registers, an 8-bit stack pointer, and 16 whole bits of addressable memory. 

The system itself is set at a nice smooth 1 MIPS. Video output is still a work in progress, but is intended to run as fast as possible for now. Eventually it'll be moved to OpenGL or something a bit more efficient than GDI. 

The display output supports 4-bit color (CGA palette), currently at a 64x64 resolution but will be increased to 128x128 in the future. Video memory is directly mapped to system memory. 

## Registers

A, B – 8 Bit - Both numerical registers used for common mathematical operations

X – 8 Bit - Stores results of ops

S – Stack pointer. 8 Bit, only the low 8 bits of the  next stack position. Push decrements it, pop increments it. 

I – Instruction counter, 16 bits, stores the current memory position for the next instruction 


## Memory Map

$0000 - $00ff - Reserved

$0100 - $01FF – Stack memory. Pointer initializes at $01FF

$0200 - $1200 – Mapped video memory

$1200 - $FFFF – Program memory

Subject to change. 

## Instructions

| Opcode | Implemented | Size | Instruction | Description |
|--------|-------------|------|-------------|-------------|
|0x00  | Y    | 1  | NOP  | No Operation |
|0x01  | Y    | 1  | ADD  | Add A, B, Store in X |
|0x02  | Y    | 1  | SUB  | Sub B from A, store in X |
|0x03  | Y    | 1  | MLT  | Mult A and B, store in X |
|0x04  | Y    | 1  | DIV  | Divide B into A, store in X |
|0x05  | Y    | 1  | DIR  | Divide A into B, store remainder in X |
|0x06  | Y    | 1  | TAB  | Transfer A to B |
|0x07  | Y    | 1  | TBA  | Transfer B to A |
|0x08  | Y    | 1  | TAX  | Transfer A to X |
|0x09  | Y    | 1  | TXA  | Transfer X to A |
|0x0A  | N    | 1  | TAS  | Transfer A to Stack pointer position |
|0x0B  | N    | 1  | TBS  | Transfer B to stack pointer position |
|0x0C  | N    | 1  | PHA  | Push A to the stack |
|0x0D  | N    | 1  | PHB  | Push B to the stack |
|0x0E  | N    | 1  | POA  | Pop A from Stack |
|0x0F  | N    | 1  | POB  | Pop B from stack |
|0x10  | Y    | 3  | LDA  | Load A with M |
|0x11  | Y    | 3  | LDB  | Load B with M |
|0x12  | Y    | 3  | LDX  | Load X with M |
|0x13  | Y    | 3  | STA  | Store A in M |
|0x14  | Y    | 3  | STB  | Store B in M |
|0x15  | Y    | 3  | STX  | Store X in M |
|0x16  | Y    | 3  | JMP  | Jump to M |
|0x17  | Y    | 1  | HLT  | Halt all execution |