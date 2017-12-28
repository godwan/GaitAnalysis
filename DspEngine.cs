using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

 namespace EcgChart
{
    class DspEngine
    {
      public  static double[] num = { 0.000198715816315835,- 0.00149631555145739,0.00166518982922744,0.00305192318741829,
            - 0.00975571446893527,0.00590606925762361,0.0151661551214688,- 0.0334758655761667,0.0117284401001423,
            0.0563731412515212,- 0.106091890199582,0.0160473047113242,0.540682846521100,0.540682846521100,
            0.016047304711324,- 0.106091890199582,0.0563731412515212,0.0117284401001423,- 0.0334758655761667,
            0.0151661551214688,0.00590606925762361,- 0.00975571446893527,0.00305192318741829,0.00166518982922744,
            - 0.00149631555145739 ,0.000198715816315835 };
        
        private static double[] xIn = {0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0,0};
        public double bcgFilter(double x) {
            return bandFilter(x);
        }

        private double bandFilter(double temp) {
            xIn[25] = temp;
            double outV= 0;
            int i;
            for (i = 0; i < 26; i++) {
                outV += num[i] * xIn[i];
            }

            for (i = 1; i < 26; i++)
            {
                xIn[i-1] = xIn[i];
            }
            return outV;
        }

    }
}
