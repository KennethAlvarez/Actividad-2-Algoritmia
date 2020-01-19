/*
 * Created by SharpDevelop.
 * User: 1GX69LA_RS4
 * Date: 07/09/2019
 * Time: 11:31 a. m.
 * 
 * To change this template use Tools | Options | Coding | Edit Standard Headers.
 */
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace AlgoritmiaAct2
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
	
		Bitmap bm;
		Grafo grafo = new Grafo();
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
		}
		void dibujarEtiqueta()
		{
			Graphics grap = Graphics.FromImage(bm);
			Font a = new Font("Arial", 35);
			SolidBrush brocha = new SolidBrush(Color.Black);
			StringFormat cad = new StringFormat();
			cad.FormatFlags = StringFormatFlags.DirectionVertical;
			for(int i = 0; i<grafo.getVertices().Count;i++)
			{
				string cadena = ""+(i);
				grap.DrawString(cadena, a, brocha, grafo.getVertices()[i].getCentro().X, grafo.getVertices()[i].getCentro().Y);
			}
			pictureBox1.Refresh();
		}
		void Button1Click(object sender, EventArgs e)
		{
			treeView1.Nodes.Clear();
			grafo.getVertices().Clear();
			listView1.Items.Clear();
			this.openFileDialog1.ShowDialog();
			string direccion = openFileDialog1.FileName;
			bm = new Bitmap(direccion);
			pictureBox1.Image = bm;
		}
		
		int obtenerRadio(Point p)
		{	
			int _x = p.X;
			int _y = p.Y;
			int contx = 0;
			int conty = 0;
			
			while(bm.GetPixel(_x, p.Y) != Color.FromArgb(255,255,255))
			{
				_x++;
				contx++;	
			}
			while(bm.GetPixel(p.X, _y) != Color.FromArgb(255,255,255))
			{
				_y++;
				conty++;
			}
			int radio = contx - conty;
			if(radio < -10 || radio > 10)
			{
				return -1;
			}
			return contx;
		}
		void Button2Click(object sender, EventArgs e)
		{
			Point centro;
			int radio = 0;
			int contador = 0;
			for(int i = 0; i<bm.Width;i++)
			{
				for(int j = 0; j<bm.Height;j++)
				{
					if(bm.GetPixel(i, j) == Color.FromArgb(0, 0, 0))
					{
						centro = obtenerCentro(i, j);
						radio = obtenerRadio(centro);
						dibujarCirculo(i, j, radio);
						if(radio != -1)
						{
							Vertice verticeAux = new Vertice(contador, centro, radio);
							grafo.agregarVertice(verticeAux);
							contador++;
						}
					}
				}
			}
			//MessageBox.Show("Contador: "+grafo.getVertices().Count);
			generarAristas();
			dibujarEtiqueta();
			ordenarVertices();
			mostrarInformacion();
			generarTreeView();
		}
		void generarTreeView()
		{
			
			treeView1.BeginUpdate();
			for(int i = 0; i<grafo.getVertices().Count;i++)
			{
				treeView1.Nodes.Add("Vertice: "+grafo.getVertices()[i].getID());
				//MessageBox.Show("Contador Vertice: "+grafo.getVertices()[i].getID()+" = "+grafo.getVertices()[i].getLista().Count);
				for(int j = 0; j<grafo.getVertices()[i].getLista().Count;j++)
				{
					treeView1.Nodes[i].Nodes.Add("Destino: "+grafo.getVertices()[i].getLista()[j].getDestino().getID());
				}
			}
			treeView1.EndUpdate();
		}
		
		bool detectarObstaculos(Point origenCentro, Point DestinoCentro)
		{
			int x = origenCentro.X;
			int y = origenCentro.Y;
			int x1 = DestinoCentro.X;
			int y1 = DestinoCentro.Y;
			
			float Dx = x1 -x;
			float Dy = y1-y;
			float m, b;
			int band = 0;
			if(Math.Abs(Dx) < Math.Abs(Dy))
			{
				if(Dy != 0)
				{
					m = Dx / Dy;
					b = x - m*y;
					if(Dy < 0)
					{
						Dy = -1;
					}
					else
					{
						Dy = 1;
					}
				while(y != y1)
				{					
					y += (int)Dy;
					x = (int)Math.Round(m*y + b);
					switch(band)
					{
						case 0:
							if(Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
								band = 1;
							break;
						case 1:
							if(!Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
							{
								band = 2;
							}
							break;
						case 2:
							if(Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
							{
								//MessageBox.Show("Origen X:"+origenCentro.X+" Y: "+origenCentro.Y+" Destino X: "+DestinoCentro.X+" Y: "+DestinoCentro.Y);
								return false;
							}
							break;
						}
						
					}
				}
			}else
			{
				m = Dy / Dx;
				b = y - m*x;
				if(Dx < 0)
				{
					MessageBox.Show("aagria");
					                
					Dx = -1;
				}
				else
				{
					Dx = 1;
				}
				while(x != x1)
				{
					x += (int)Dx;
					y = (int)Math.Round(m*x + b);
					switch(band)
					{
						case 0:
							if(Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
								band = 1;
							break;
						case 1:
							if(!Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
							{
								band = 2;
							}
							break;
						case 2:
							if(Color.Equals(Color.FromArgb(255,255,255), bm.GetPixel(x, y)))
							{
								return false;
							} break;
					}
					
				}
			}
			return true;
	}
		void generarAristas()
		{
			int ponderacion = 0;
			int j = 0;
			int generarID = 0;
			Arista aux = new Arista(-1, null, null, 5000);
			for(int i = 0; i<grafo.getVertices().Count;i++)
			{
				for(j = i+1; j<grafo.getVertices().Count;j++)
				{
					ponderacion = obtenerDistancia(grafo.getVertices()[i].getCentro(), grafo.getVertices()[j].getCentro());
					
					if(ponderacion < aux.getPonderacion())
					{
						aux = new Arista(-1, grafo.getVertices()[i], grafo.getVertices()[j], ponderacion);
					}
					if(detectarObstaculos(grafo.getVertices()[i].getCentro(), grafo.getVertices()[j].getCentro()))
					{
						Arista arista = new Arista(generarID, grafo.getVertices()[i], grafo.getVertices()[j], ponderacion);
						grafo.getVertices()[i].agregarArista(arista);
						generarID++;
						Arista _arista = new Arista(generarID, grafo.getVertices()[j], grafo.getVertices()[i], ponderacion);
						grafo.getVertices()[j].agregarArista(_arista);
						generarID++;
						pictureBox1.Refresh();	
						//MessageBox.Show("Origen "+grafo.getVertices()[j].getLista()[0].getOrigen().getID());
						//MessageBox.Show("Destino: "+grafo.getVertices()[j].getLista()[0].getDestino().getID());
					}
					
				}
			}
			
			if(grafo.getVertices().Count > 1)
			{
				dibujarCentro(aux.getOrigen().getCentro().X, aux.getOrigen().getCentro().Y);
				dibujarCentro(aux.getDestino().getCentro().X, aux.getDestino().getCentro().Y);
			}
			else
			{
				MessageBox.Show("Debe haber como mínimo 2 vértices para conectar");	
			}
			pictureBox1.Refresh();
			pintarAristas();
		}
		void ordenarVertices()
		{
			Vertice v_1;
			for(int i = 0; i<grafo.getVertices().Count;i++)
			{
				for(int j = 0; j<grafo.getVertices().Count - 1; j++)
				{
					if(grafo.getVertices()[j].getRadio() > grafo.getVertices()[j+1].getRadio())
					{
						v_1 = grafo.getVertices()[j+1];
						grafo.getVertices()[j+1] = grafo.getVertices()[j];
						grafo.getVertices()[j] = v_1;
					}
				}
			}
		}
		void pintarAristas()
		{
			Pen lapiz = new Pen(Color.Red, 3);
			Graphics gr = Graphics.FromImage(bm);
			for(int i = 0; i<grafo.getVertices().Count;i++)
			{
				for(int j = 0; j<grafo.getVertices()[i].getLista().Count;j++)
				{
					if(grafo.getVertices()[i].getID() < grafo.getVertices()[i].getLista()[j].getDestino().getID());
					gr.DrawLine(lapiz, grafo.getVertices()[i].getCentro(),grafo.getVertices()[i].getLista()[j].getDestino().getCentro());
					
				}
				pictureBox1.Refresh();
			}
			
		}
		int obtenerDistancia(Point origenCentro, Point destinoCentro)
		{
			int distancia = (int)Math.Round(Math.Sqrt(Math.Pow((destinoCentro.X - origenCentro.X), 2) + Math.Pow((destinoCentro.Y - origenCentro.Y), 2)));
			return distancia;		
		}
		void dibujarCentro(int x, int y)
		{
			int k = 10;

	   		for(int i = -k; i<k;i++)
	   		{
	   			for(int j = -k; j<k;j++)
	   			{
	   				bm.SetPixel(x+i, y+j, Color.Pink);	   
	   			}	   	
	   		}

		} 
		Point obtenerCentro(int x, int y)
		{
			int aux=0;
			int auxy=0;
			aux = x;
			auxy = y;
			//MessageBox.Show("antes x: "+aux+" y: "+auxy);
			while(bm.GetPixel(aux, auxy) == Color.FromArgb(0, 0, 0))
			{
				aux++;
			}
			int xTotal = (aux-x)/2;
			aux = x;
			while(bm.GetPixel(aux, auxy) == Color.FromArgb(0, 0, 0))
			{
				auxy++;
			}
			int yTotal = (auxy-y)/2;
			Point centro = new Point((xTotal+x), (yTotal+y));
			//MessageBox.Show("Total: "+(xTotal+x)+" "+(yTotal+y));
			return centro;
			
		}
		void dibujarCirculo(int x, int y, int radio)
		{
			int _x = x;
			int _y = y;
			Color col;
			if(radio == -1)
			{			
				//MessageBox.Show("x: "+x+" Y: "+y);
				col = Color.FromArgb(255,255,255);
			}else
			{
				col = Color.Blue;
			}
			while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
			{
				while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
				{
					bm.SetPixel(_x, _y, col);
					_y--;
				}
				//pictureBox1.Refresh();
				_x++;
				_y = y;
			}
			_x = x-1;
			_y = y;
			while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
			{
				while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
				{
					bm.SetPixel(_x, _y, col);
					_y--;
				}
				//pictureBox1.Refresh();
				_x--;
				_y = y;
			}
			_x = x;
			_y = y+1;
			while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
			{
				while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
				{
					bm.SetPixel(_x, _y, col);
					_y++;
				}
				//pictureBox1.Refresh();
				_x++;
				_y = y+1;
			}
			_x = x-1;
			_y = y+1;
			while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
			{
				while(bm.GetPixel(_x, _y) != Color.FromArgb(255, 255, 255))
				{
					bm.SetPixel(_x, _y, col);
					_y++;
				}
				//pictureBox1.Refresh();
				_x--;
				_y = y;
			}
			pictureBox1.Refresh();

		}
		void mostrarInformacion()
		{
			listView1.Items.Clear();
			int i =0;
			while(i<grafo.getVertices().Count)
			{
				ListViewItem item = new ListViewItem(grafo.getVertices()[i].getID().ToString());
				item.SubItems.Add(grafo.getVertices()[i].getCentro().X.ToString());
				item.SubItems.Add(grafo.getVertices()[i].getCentro().Y.ToString());
				item.SubItems.Add(grafo.getVertices()[i].getRadio().ToString());
				
				listView1.Items.Add(item);
				i++;
			}
		}
	}
}
