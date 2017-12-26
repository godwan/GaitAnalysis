using System;
using System.Collections.Generic;
using System.Linq;
namespace EcgChart
{
	/// <summary>
	/// Description of IncomingData.
	/// </summary>
	public class IncomingData
	{ public static double G = 9.8;
		public IncomingData()
		{
		}
		public static IEnumerable<SerialDataEntity> parseBytes(byte[] data)
		{
			int ByteLength = 10;
			List<string> cByte = new List<string>();
			for (int i=0, length=data.Length; i<length; i++) 
			{
				if(data[i]==85) {
                    if (i <= length - ByteLength && data[i + 1] == 81)
                    {       //解析的是加速度包					
                        if (cByte.Count == ByteLength)
                        {
                            yield return parseByteBySpeed(cByte);
                        }
                        cByte.Clear();
                    }
                    else if (i <= length - ByteLength && data[i + 1] == 82) //解析的是角速度包
                    {
                        if (cByte.Count == ByteLength)
                        {
                            yield return parseByteByAngle_velocity(cByte);
                        }
                        cByte.Clear();
                    }
                    else if (i <= length - ByteLength && data[i + 1] == 83)// 解析的是角度包
                    {
                        if (cByte.Count == ByteLength)
                        {
                            yield return parseByteByAngle(cByte);
                        }
                        cByte.Clear();
                    }
                    
                }
				else 
				{
					cByte.Add(data[i].ToString());
				}
			}
		}

        static SerialDataEntity parseByteBySpeed(List<string> cByte)
        {
            SerialDataEntity temp = new SerialDataEntity();
            double wx = (double.Parse(cByte[2]) * 256 + double.Parse(cByte[1])) / 32768 * 16 * G; // 度/s   x方向的加速度
            double wy = (double.Parse(cByte[4]) * 256 + double.Parse(cByte[3])) / 32768 * 16 * G; // 度/s   y方向的加速度
            double wz = (double.Parse(cByte[6]) * 256 + double.Parse(cByte[5])) / 32768 * 16 * G; // 度/s   z方向的加速度
            double temperture = (double.Parse(cByte[8]) * 256 + double.Parse(cByte[7])) / 340 + 36.53; //温度  摄氏度

            temp.setNum(1);
            temp.setX(wx);
            temp.setY(wy);
            temp.setZ(wz);
            temp.setTem(temperture);
            return temp;
        }

        static SerialDataEntity parseByteByAngle_velocity(List<string> cByte)
        {
            SerialDataEntity temp = new SerialDataEntity();
            double wx = (double.Parse(cByte[2]) * 256 + double.Parse(cByte[1])) / 32768 * 2000; // 度/s   x方向的角速度
            double wy = (double.Parse(cByte[4]) * 256 + double.Parse(cByte[3])) / 32768 * 2000; // 度/s   y方向的角速度
            double wz = (double.Parse(cByte[6]) * 256 + double.Parse(cByte[5])) / 32768 * 2000; // 度/s   z方向的角速度
            double temperture = (double.Parse(cByte[8]) * 256 + double.Parse(cByte[7])) / 340 + 36.53; //温度  摄氏度

            temp.setNum(2);
            temp.setX(wx);
            temp.setY(wy);
            temp.setZ(wz);
            temp.setTem(temperture);
            return temp;
        }

        static SerialDataEntity parseByteByAngle(List<string> cByte) {
            SerialDataEntity temp = new SerialDataEntity();
            double wx = (double.Parse(cByte[2]) * 256 + double.Parse(cByte[1])) / 32768 * 180; // 度  x方向的角度
            double wy = (double.Parse(cByte[4]) * 256 + double.Parse(cByte[3])) / 32768 * 180; // 度   y方向的角度
            double wz = (double.Parse(cByte[6]) * 256 + double.Parse(cByte[5])) / 32768 * 180; // 度   z方向的角度
            double temperture = (double.Parse(cByte[8]) * 256 + double.Parse(cByte[7])) / 340 +36.53; //温度  摄氏度

            temp.setNum(3);
            temp.setX(wx);
            temp.setY(wy);
            temp.setZ(wz);
            temp.setTem(temperture);
            return temp;
        }


        static double parseByte(List<string> cByte)
		{
//			if(!checkSum(cByte)) return;

			double v1 = double.Parse(cByte[3]);
			double v2 = double.Parse(cByte[4]);
           
            double v = v1*16*16+v2;
			v = hexToSigned(v);
            testCheckSum();
            return v;
		}
		static void testCheckSum()
		{
			List<int> foo = new List<int>();
			foo.Add(128);
			foo.Add(2);
			foo.Add(9);
			foo.Add(82);
			foo.Add(34);
			string msg = checkSum(foo)?"ok":"nope";
//			MessageBox.Show(msg);
		}
		static bool checkSum(List<int> arr)
		{
			int length, checkValue, checksum, xx;
			length = arr.Count;
			if(length<2) return false;
			checkValue = arr[length-1]; arr.RemoveAt(length-1); //Array.pop();
			checksum = arr.Sum(); 
			checksum &= 0xFF;
            xx = ~checksum;
            xx = ~0;
            checksum = ~checksum & 0xFF;
			return checksum==checkValue;
		}
		static double hexToSigned(double vd) {
			int v = (int)vd;
			if ((v & 0x8000) > 0) {
				v = v - 0x10000;
			}
			return v;
		}
	}
}
