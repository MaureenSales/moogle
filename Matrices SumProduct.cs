using System;

namespace MiEval;

class MatrizOps
{

    public double[,] matriz { get; set; }
    public int filas { get; set; }
    public int columnas { get; set; }
    public MatrizOps(double[,] a)
    {
        this.filas = filas;
        this.columnas = columnas;
        matriz = new double[filas, columnas];
    }
    public double this[int i, int j]
    {
        get
        {
            return matriz[i, j];
        }
        set
        {
            matriz[i, j] = value;
        }
    }
    //se debe evaluar si cumple la condicion de multiplicatividad antes de realizar el producto
    public static bool Multiplicable(MatrizOps a, MatrizOps b)
    {
        if (a.columnas == b.filas)
        {
            return true;
        }
        else return false;
    }
    //antes de sumar se debe evaluar si las matrices poseen igual orden
    public static bool IgualOrden(MatrizOps a, MatrizOps b)
    {
        if((a.filas == b.filas) && (a.columnas == b.columnas))
        {
            return true;
        }
        else return false;
    }
    public static MatrizOps Producto(MatrizOps a, MatrizOps b)
    {
        double[,] producto = new double[a.filas, b.columnas];
        for (int i = 0; i < a.filas; i++)
        {
            for (int j = 0; j < b.columnas; j++)
            {
                producto[i, j] = 0;
                for (int k = 0; k < a.columnas; k++)
                {
                    producto[i, j] += a[i, k] * b[k, j];
                }
            }
        }
        return new MatrizOps(producto);
    }
    public static MatrizOps Producto_Escalar(MatrizOps a, double b)
    {
        double[,] producto_escalar = new double[a.filas, a.columnas];
        for (int i = 0; i < a.filas; i++)
        {
            for (int j = 0; j < a.columnas; j++)
            {
                producto_escalar[i,j] = b * a[i,j];
            }
        }
        return new MatrizOps(producto_escalar);
    }

    public static MatrizOps Suma(MatrizOps a, MatrizOps b)
    {
        double[,] suma = new double[a.filas, b.columnas];
         for (int i = 0; i < a.filas; i++)
        {
            for (int j = 0; j < b.columnas; j++)
            {
                suma[i,j] = a[i,j] + b[i,j];
            }
        }
        return new MatrizOps(suma);
    }
}