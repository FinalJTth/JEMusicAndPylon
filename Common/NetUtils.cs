using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using Terraria;
using Terraria.ID;
using Microsoft.Xna.Framework;
using Terraria.Localization;

namespace JEMusicAndPylon.Common
{
    public static class NetUtils
    {
        public static void WriteList<T>(List<T> value, BinaryWriter writer)
        {
            writer.Write("startList");
            for (int i = 0; i < value.Count; i++)
            {
                switch (Type.GetTypeCode(typeof(T)))
                {
                    //_writer.Write(value[i]);
                    case TypeCode.Boolean:
                        writer.Write((bool)(object)value[i]);
                        break;
                    case TypeCode.Byte:
                        writer.Write((byte)(object)value[i]);
                        break;
                    case TypeCode.Char:
                        writer.Write((char)(object)value[i]);
                        break;
                    case TypeCode.Decimal:
                        writer.Write((decimal)(object)value[i]);
                        break;
                    case TypeCode.Double:
                        writer.Write((double)(object)value[i]);
                        break;
                    case TypeCode.Single:
                        writer.Write((float)(object)value[i]);
                        break;
                    case TypeCode.Int16:
                        writer.Write((short)(object)value[i]);
                        break;
                    case TypeCode.Int32:
                        writer.Write((int)(object)value[i]);
                        break;
                    case TypeCode.Int64:
                        writer.Write((short)(object)value[i]);
                        break;
                    case TypeCode.String:
                        writer.Write((string)(object)value[i]);
                        break;
                    case TypeCode.SByte:
                        writer.Write((sbyte)(object)value[i]);
                        break;
                    case TypeCode.UInt16:
                        writer.Write((ushort)(object)value[i]);
                        break;
                    case TypeCode.UInt32:
                        writer.Write((uint)(object)value[i]);
                        break;
                    case TypeCode.UInt64:
                        writer.Write((ulong)(object)value[i]);
                        break;
                    default:
                        if (typeof(T) == typeof(byte[]))
                        {
                            writer.Write((byte[])(object)value[i]);
                        }
                        else if (typeof(T) == typeof(char[]))
                        {
                            writer.Write((char[])(object)value[i]);
                        }
                        else
                        {
                            throw new ArgumentException("List type not supported");
                        }

                        break;
                }
            }
            writer.Write("stopList");
        }
    }
}
