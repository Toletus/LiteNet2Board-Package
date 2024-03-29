﻿using System;
using System.Linq;
using Toletus.LiteNet2.Command.Enums;
using Toletus.Pack.Core.Extensions;

namespace Toletus.LiteNet2.Command;

public class LiteNet2Response
{
    public LiteNet2Response(byte[] response)
    {
        /* Payload (20 bytes)
         *
         * /- Prefix (1 byte) (0x53)
         * |
         * |/- Command (2 bytes)
         * ||
         * || /- Data (16 bytes)
         * || |                         
         * || |               /- Suffix (1 byte) (0xc3)
         * || |               |
         * 01234567890123456789
        */

        Payload = response;
        Command = (LiteNet2Commands)BitConverter.ToUInt16(response, 1);
        RawData = response.Skip(3).Take(16).ToArray();
    }

    public byte[] Payload { get; set; }
    public LiteNet2Commands Command { get; }
    public byte[] RawData { get; }
    public ushort Data => BitConverter.ToUInt16(RawData, 0);
    public string DataString => RawData.SupressEndWithZeroBytes().ConvertToAsciiString().Trim();
    public Identification? Identification { get; set; }

    public override string ToString()
    {
        try
        {
            var ret = $"[{Payload.ToHexString(" ")}] {Data} {DataString} {Command}";

            return ret;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}