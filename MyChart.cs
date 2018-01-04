using System;
using System.Collections.Generic;
using System.Drawing;
using ZedGraph;
namespace EcgChart
{
	/// <summary>
	/// Class for drawing my chart with ZedGraph
	/// </summary>
	public class MyChart
	{
		ZedGraphControl graph;
		GraphPane pane;
		const string chartTitle = "原始数据";
		int pointCount;
        Dictionary<int, int> _dicPointCount = new Dictionary<int, int>();
		
		public MyChart(ZedGraphControl g)
		{
			graph = g; 
			pane = graph.GraphPane; 
			pointCount = 0; 
			initGraph();
            _dicPointCount.Add(0, 0);
            _dicPointCount.Add(1, 0);
            _dicPointCount.Add(2, 0);
        }
		void initGraph(){
			pane.CurveList.Clear();
			pane.Title.Text = chartTitle;
			pane.YAxis.Title.IsVisible = false;
			pane.XAxis.Title.Text="Time";
//			pane.XAxis.Title.IsVisible = false;
			pane.XAxis.Scale.Min = 0;
			graph.RestoreScale(pane); // restore to default all zooming, etc.
//			pane.XAxis.AxisGap
			LineItem myCurve = pane.AddCurve ("x", null, Color.Blue, SymbolType.None);
            pane.AddCurve("y", null, Color.Red, SymbolType.None);
            pane.AddCurve("z", null, Color.Green, SymbolType.None);
            graph.IsEnableHZoom = true;
			graph.IsEnableVZoom = false;
			graph.IsShowHScrollBar = true;
			graph.IsAutoScrollRange = true;
			Update();
		}
		public void DrawGraph (List<double> val,string label,bool append)
		{
			label = label ?? "ECG";
			Color color = Color.Blue;
			
			if(!append) pane.CurveList.Clear (); 
			else color=Color.Maroon;
			pointCount = 0;
			PointPairList list = listToPointPairList(val); 
			LineItem myCurve = pane.AddCurve (label, list, color, SymbolType.None);
			graph.RestoreScale(pane);
			Update();
		}
		PointPairList listToPointPairList(List<double> val)
		{
			PointPairList result = new PointPairList ();
			int l=val.Count;
			for(int i=0; i<l;i++) {
				result.Add(i,val[i]);
			}
			return result;
		}
		void Update() 
		{
			graph.AxisChange ();
			graph.Invalidate ();
		}
		public void AddPoint(double val,int curveIndex=0) 
		{
            if(pane.CurveList[curveIndex].Points.Count >= 600)
            {
                pane.CurveList[curveIndex].RemovePoint(0);
                pane.XAxis.Scale.Min = pane.CurveList[curveIndex].Points[0].X;
            }
            var maxIndex = _dicPointCount[curveIndex]++;
            pane.CurveList[curveIndex].AddPoint(maxIndex,val);
            pane.XAxis.Scale.Max = maxIndex;
            Update();
		}
	}
}
