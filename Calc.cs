using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

// 255.255.0.0
namespace IPCalc
{
    public class Calc
    {
        public string IP { get; }
        public string Mask { get; }

        public void Devider()
        {
            var ipBytes = Parser(this.IP);
            var maskBytes = Parser(this.Mask);
            var converter = this.Converter(maskBytes);

            CheckMask(converter);

            var id = GetID(ipBytes, maskBytes);
            Console.WriteLine("Network id:"+id.Item1 + "\nHost id:" + id.Item2);
            Console.WriteLine("Количество узлов в сети:" + HostsCount(maskBytes));
        }

        public byte[] Parser(string ip)
        {
            var bytes = ip.Split('.');
            if (bytes.Length != 4) throw new ArgumentException("неверное количество октетов");
            var ConvertedBytes = new byte[4];
            for (int i = 0; i < 4; i++)
            {
                if (byte.TryParse(bytes[i], out byte octet))
                {
                    ConvertedBytes[i] = octet;
                }
                else throw new ArgumentException("неверное значение октета ");
            }
            return ConvertedBytes;
        }

        public string[] Converter(byte[] bytes)
        {
            
            var ConvertedBytes = new string[4];
            for (int i = 0; i < 4; i++)
            {
                ConvertedBytes[i] = this.BitConverter(bytes[i]);
            }

            return ConvertedBytes;
        }

        public void CheckMask(string[] octets)
        {

            bool flag = true;
            foreach (var octet in octets)
            {
                foreach (var bit in octet.ToCharArray())
                {
                    if (!flag && bit.Equals('1')) throw new ArgumentException("Wrong mask!!!");
                    if (bit.Equals('0')) flag = false;
                }
            }

        }

        public Tuple<string, string> GetID(byte[] id, byte[] mask)
        {
            var networkId = new byte[4];
            var hostId = new byte[4];

            for (int i = 0; i < 4 ; i++)
            {
                networkId[i] = (byte)(id[i] & mask[i]);
                hostId[i] = (byte)((id[i] & ~mask[i]));
            }
            var networkIdJoin = new StringBuilder();
            var hostIdJoin = new StringBuilder();
            for (int i = 0; i < 4; i++)
            {
                networkIdJoin.Append(networkId[i].ToString() + ".");
                hostIdJoin.Append(hostId[i].ToString() + ".");
            }

            networkIdJoin.Remove(networkIdJoin.Length - 1, 1);
            hostIdJoin.Remove(hostIdJoin.Length - 1, 1);

            return new Tuple<string, string>(networkIdJoin.ToString(), hostIdJoin.ToString());
        }

        public Calc(string ip, string mask)
        {
            IP = ip;
            Mask = mask;
        }

        public string BitConverter(byte number)
        {
            string result = string.Empty;
            for (int i = 7; i >= 0; i--)
            {
                result += (((number >> i) & 1) == 0) ? "0" : "1";
            }
            return result;
        }

        public int HostsCount(byte[] mask)
        {
            string maskConvert = String.Empty;
            
            for (int i = 0; i < 4; i++)
            {
                maskConvert += BitConverter(mask[i]);   
            }
            var count = 0;
            for (int i = 31; i >= 0; i--)
            {
                if (maskConvert[i] == '1') break;
                count++;
            }
            return (int)Math.Pow(2.0,(double)count) - 2;
        }
    }
}
