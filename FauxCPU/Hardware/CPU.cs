using System;
using System.Threading;
using System.Timers;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FauxCPU.Hardware
{
     class CPU
    {
        byte[] memory = new byte[0xFFFF];

        byte reg_A = 0x00;
        byte reg_B = 0x00;
        byte reg_X = 0x00;
        byte reg_S = 0xFF;
        ushort reg_I = 0x1200;

        bool isHalted = false;
        int instructionsPerSecond = 1000000;
        int cyclePeriodsMillisecond = 50;
        int instructionsPerPeriod = 0;
        long nextRunMilliseconds = 0;

        int recordedIPS = 0;

        public void Initialize()
        {
            for(int i = 0;i < memory.Length; i++)
            {
                memory[i] = 0;
            }

            nextRunMilliseconds = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
            instructionsPerPeriod = instructionsPerSecond / (1000 / cyclePeriodsMillisecond);

            System.Console.WriteLine("Expected IPS: " + instructionsPerSecond);
            System.Console.WriteLine("Per Period: " + instructionsPerPeriod);

        }

        public byte Peek(int pos)
        {
            return memory[pos];
        }

        public void Poke(int pos, byte value)
        {
            memory[pos] = value;
        }

        public void Bootup()
        {
            Thread t = new Thread(new ThreadStart(Run));
            t.IsBackground = true;

            t.Start();

            System.Timers.Timer aTimer = new System.Timers.Timer(1000);
            aTimer.Elapsed += IPSTimer;
            aTimer.AutoReset = true;
            aTimer.Enabled = true;
        }

        public void IPSTimer(Object source, ElapsedEventArgs e)
        {
            System.Console.WriteLine("IPS: " + recordedIPS);
            recordedIPS = 0;
        }

        public void Run()
        {
            while(true) {
                int sleepTime = DoExecutionPeriod();

               // System.Console.WriteLine("Next in: " + sleepTime);

                Thread.Sleep(sleepTime);
            }
            
        }


        public int DoExecutionPeriod()
        {
            for(int i = 0;i < instructionsPerPeriod; i++)
            {
                DoCycle();
            }

            long curTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

            if (nextRunMilliseconds > curTime) {
                nextRunMilliseconds = curTime;
            }

            nextRunMilliseconds += cyclePeriodsMillisecond;
   
            return (int)(nextRunMilliseconds - curTime);
        }

        public void DoCycle()
        {
            recordedIPS++;

            if (isHalted)
            {
                return;
            }

            byte opcode = Peek(reg_I);

            //System.Console.WriteLine(reg_I.ToString("X4") + ": " + opcode.ToString("X2"));
            
            reg_I++;

            switch (opcode)
            {
                case 0x00:
                    Instruction_NOP();
                    break;
                case 0x01:
                    Instruction_ADD();
                    break;
                case 0x02:
                    Instruction_SUB();
                    break;
                case 0x06:
                    Instruction_TAB();
                    break;
                case 0x07:
                    Instruction_TBA();
                    break;
                case 0x08:
                    Instruction_TAX();
                    break;
                case 0x09:
                    Instruction_TXA();
                    break;
                case 0x10:
                    Instruction_LDA();
                    break;
                case 0x11:
                    Instruction_LDB();
                    break;
                case 0x12:
                    Instruction_LDX();
                    break;
                case 0x13:
                    Instruction_STA();
                    break;
                case 0x14:
                    Instruction_STB();
                    break;
                case 0x15:
                    Instruction_STX();
                    break;
                case 0x16:
                    Instruction_JMP();
                    break;
                case 0x17:
                    Instruction_HLT();
                    break;
            }
        }

        public void Instruction_HLT()
        {
            //System.Console.WriteLine("Halted");

            isHalted = true;
        }

        public void Instruction_NOP()
        {
           // System.Console.WriteLine("NOP");
        }

        public void Instruction_JMP()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            System.Console.WriteLine("JMP: " + loc.ToString("X4"));

            reg_I = loc;
        }

        public void Instruction_ADD()
        {
            reg_X = (byte)(reg_A + reg_B);
        }

        public void Instruction_SUB()
        {
            reg_X = (byte)(reg_A - reg_B);
        }

        public void Instruction_TAB()
        {
            reg_B = reg_A;
        }

        public void Instruction_TBA()
        {
            reg_A = reg_B;
        }

        public void Instruction_TAX()
        {
            reg_X = reg_A;
        }

        public void Instruction_TXA()
        {
            reg_A = reg_X;
        }

        public void Instruction_LDA()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            reg_A = this.Peek(loc);
        }

        public void Instruction_LDB()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            reg_B = this.Peek(loc);
        }

        public void Instruction_LDX()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            reg_X = this.Peek(loc);
        }

        public void Instruction_STA()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            this.Poke(loc, reg_A);
        }

        public void Instruction_STB()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            this.Poke(loc, reg_B);
        }

        public void Instruction_STX()
        {
            ushort loc = (ushort)(Peek(reg_I) ^ (Peek(reg_I + 1) << 8));

            reg_I += 2;

            this.Poke(loc, reg_X);
        }
    }
}
