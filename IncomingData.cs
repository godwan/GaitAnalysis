﻿using System;
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
			int ByteLength = 11;
			List<string> cByte = new List<string>();
            int count = 0;
			for (int i=0, length =data.Length; i<length; i++) 
			{
                if (count < ByteLength-1)
                {
                    cByte.Add(data[i].ToString());
                    count++;
                }
                else
                {
                    cByte.Add(data[i].ToString());
                    count++;
                    if (int.Parse(cByte[0]) == 85 && int.Parse(cByte[1]) == 81)
                    {
                        if (cheakSum(cByte, ByteLength))
                        {
                            yield return parseByteBySpeed(cByte);
                            cByte.Clear();
                            count = 0;
                        }
                        else {
                            cByte.RemoveAt(0);
                            count--;
                        }
                    }
                    else if (int.Parse(cByte[0]) == 85 && int.Parse(cByte[1]) == 82) //解析的是角速度包
                    {
                        if (cheakSum(cByte, ByteLength))
                        {
                            yield return parseByteByAngle_velocity(cByte);
                            cByte.Clear();
                            count = 0;
                        }
                        else
                        {
                            cByte.RemoveAt(0);
                            count--;
                        }
                    }
                    else if (int.Parse(cByte[0]) == 85 && int.Parse(cByte[1]) == 83)// 解析的是角度包
                    {
                        if (cheakSum(cByte, ByteLength))
                        {
                            yield return parseByteByAngle(cByte);
                            cByte.Clear();
                            count = 0;
                        }
                        else
                        {
                            cByte.RemoveAt(0);
                            count--;
                        }
                    }
                    else
                    {
                        cByte.RemoveAt(0);
                        count--;
                    }
                }
                   
			}
		}

        static SerialDataEntity parseByteBySpeed(List<string> cByte)
        {
            SerialDataEntity temp = new SerialDataEntity();

             double wx = makeSign(int.Parse(cByte[3]), int.Parse(cByte[2])) / 32768 * 16;// * G; // m/s^2   x方向的加速度
            double wy = makeSign(int.Parse(cByte[5]), int.Parse(cByte[4])) / 32768 * 16;// * G; // m/s^2   y方向的加速度
            double wz = makeSign(int.Parse(cByte[7]), int.Parse(cByte[6])) / 32768 * 16;// * G; // m/s^2  z方向的加速度
            double temperture = makeSign(int.Parse(cByte[9]), int.Parse(cByte[8])) / 340 + 36.53; //温度  摄氏度
            Console.WriteLine("加速度：x方向"+wx.ToString()+"  y方向"+ wy.ToString()+"  z方向"+ wz.ToString());

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
            double wx = makeSign(int.Parse(cByte[3]), int.Parse(cByte[2])) / 32768 * 2000; // 度/s   x方向的角速度
            double wy = makeSign(int.Parse(cByte[5]), int.Parse(cByte[4])) / 32768 * 2000; // 度/s   y方向的角速度
            double wz = makeSign(int.Parse(cByte[7]), int.Parse(cByte[6])) / 32768 * 2000; // 度/s   z方向的角速度
            double temperture = makeSign(int.Parse(cByte[9]), int.Parse(cByte[8])) / 340 + 36.53; //温度  摄氏度
            //Console.WriteLine("角速度：x方向角速度", wx, "y方向角速度", wy, "z方向角速度", wz);
            temp.setNum(2);
            temp.setX(wx);
            temp.setY(wy);
            temp.setZ(wz);
            temp.setTem(temperture);
            return temp;
        }

        static SerialDataEntity parseByteByAngle(List<string> cByte) {
            SerialDataEntity temp = new SerialDataEntity();
            double wx = makeSign(int.Parse(cByte[3]), int.Parse(cByte[2])) / 32768 * 180; // 度  x方向的角度
            double wy = makeSign(int.Parse(cByte[5]), int.Parse(cByte[4])) / 32768 * 180; // 度   y方向的角度
            double wz = makeSign(int.Parse(cByte[7]), int.Parse(cByte[6])) / 32768 * 180; // 度   z方向的角度
            double temperture = makeSign(int.Parse(cByte[9]), int.Parse(cByte[8])) / 340 +36.53; //温度  摄氏度

            temp.setNum(3);
            temp.setX(wx);
            temp.setY(wy);
            temp.setZ(wz);
            temp.setTem(temperture);
            return temp;
        }
        /**
         * 求数据的符号位
         */
        static double makeSign(int highByte, int lowByte)
        {
            double outD = 0;
            if (highByte > 127) // 取补码
            {
                //outD = -1 * (127 - (highByte - 128)) * 256 + 255 - lowByte + 1;
                //outD = -1 * (255 - highByte) * 256 + 256 - lowByte;
                outD = -1 * (254 - highByte) * 256 - lowByte;
            }
            else//整数
            {
                outD = (highByte * 256 + lowByte);
            }
            return outD;
        }

        /**
         * 校验和 
         */
        static bool cheakSum(List<string> cByte,int len) {
            int sum = 0;
            bool flag = false;
            for(int i=0;i < len-1; i++){
                sum += int.Parse(cByte[i]);
            }

            if (sum % 256 == int.Parse(cByte[len - 1]))
            {
                flag =  true;
            }
            return flag;
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
