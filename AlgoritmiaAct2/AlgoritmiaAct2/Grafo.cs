/*
 * Created by SharpDevelop.
 * User: 1GX69LA_RS4
 * Date: 15/09/2019
 * Time: 12:07 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;

namespace AlgoritmiaAct2
{
	/// <summary>
	/// Description of Grafo.
	/// </summary>
	public class Grafo
	{
		List<Vertice> vL;
		public Grafo()
		{
			vL = new List<Vertice>();
		}
		public void agregarVertice(Vertice v)
		{
			this.vL.Add(v);
		}
		public List<Vertice> getVertices()
		{
			return this.vL;
		}
	}
	public class Vertice
	{
		int id;
		List<Arista> aL;
		Point centro;
		int radio;
		public Vertice(int ide, Point cntr, int r)
		{
			this.id = ide;
			this.centro = new Point(cntr.X, cntr.Y);
			this.radio = r;
			this.aL = new List<Arista>();
		}
		public int getRadio()
		{
			return radio;
		}
		public void agregarArista(Arista l)
		{
			this.aL.Add(l);
		}
		public int getID()
		{
			return this.id;
		}
		public List<Arista> getLista()
		{
			return this.aL;
		}
		public Point getCentro()
		{
			return centro;
		}
	}
	public class Arista
	{
		int id;
		Vertice origen;
		Vertice destino;
		int ponderacion;
		public Arista(int ide, Vertice v_1, Vertice v_2, int p)
		{
			this.id = ide;
			origen = v_1;
			destino = v_2;
			ponderacion = p;
		}
		public Vertice getOrigen()
		{
			return origen;
		}
		public Vertice getDestino()
		{
			return destino;
		}
		public int getPonderacion()
		{
			return ponderacion;
		}
		public int getID()
		{
			return id;
		}
		public void setPonderacion(int dato)
		{
			ponderacion = dato;
		}
	}
}
