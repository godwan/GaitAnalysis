using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EcgChart
{
    public class SerialDataEntity
    {
        private double num =0;//初始化
        private double x =0;
        private double y =0;
        private double z =0;
        private double tem=0;
        private string date = "";
        private string time = "";
        public SerialDataEntity() { }
        public SerialDataEntity(double num,double x,double y,double z,double tem,string date,string time) {
            this.num = num;
            this.x = x;
            this.y = y;
            this.z = z;
            this.tem = tem;
            this.date = date;
            this.time = time;
        }

        public void setNum(double num) {
            this.num = num;
        }
        public double getNum()
        {
            return num;
        }
        public void setX(double x)
        {
            this.x = x;
        }
        public double getX()
        {
            return x;
        }
        public void setY(double y)
        {
            this.y = y;
        }
        public double getY()
        {
            return y;
        }
        public void setZ(double z)
        {
            this.z = z;
        }
        public double getZ()
        {
            return z;
        }
        public void setTem(double tem)
        {
            this.tem = tem;
        }
        public double getTem()
        {
            return tem;
        }
        public void setDate(string date)
        {
            this.date = date;
        }
        public string getDate()
        {
            return date;
        }

        public void setTime(string time)
        {
            this.time = time;
        }
        public string getTime()
        {
            return time;
        }

    }
}
